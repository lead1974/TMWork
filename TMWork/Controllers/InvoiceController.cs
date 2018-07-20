using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TMWork.Services;
using Microsoft.Extensions.Logging;
using TMWork.Data.Models.User;
using TMWork.Data.Models.Customer;
using TMWork.Data.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using TMWork.Data.Models.Invoice;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using TMWork.ViewModels.Invoice;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TMWork.Controllers
{
    //[Area("Admin")]
    //[Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite +","+ RoleName.CanManageInvoices)]
    public class InvoiceController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private IInvoiceRepository _invoiceRepo;

        private const int PostsPerPage = 5;
        private GlobalService _globalService;
        private readonly IHostingEnvironment _env;
        private Invoice _inv;

        public InvoiceController(
            IInvoiceRepository invoiceRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            GlobalService globalService,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            _invoiceRepo = invoiceRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _globalService = globalService;
            _env = env;
            _logger = loggerFactory.CreateLogger<InvoiceController>();
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.SelectiveTab = "invoice";
            return View();
        }
        [HttpGet, Route("CreateInvoice")]
        public IActionResult CreateInvoice()
        {
            return View("CreateEditInvoice",new Invoice());
        }

        [HttpGet, Route("CreateEditInvoice/{invoiceId?}")]
        public IActionResult CreateEditInvoice(int invoiceId)
        {
            var invoice = _invoiceRepo.FindById(invoiceId);
            _inv = invoice;

            return View("CreateEditInvoice", invoice);
        }

        [HttpPost, Route("CreateEditInvoice")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEditInvoice(Invoice model, List<IFormFile> files, string submit,string filename)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            switch (submit)
            {
                case "CreateEditInvoice":
                    break;
                case "RemoveFile":
                    var fileName = filename;
                    this.RemoveFile(model.InvoiceId, filename);
                    return View(model);
            }

            var emailSubj = string.Empty;
            if (model.InvoiceId==0)
            {
                model.CreatedBy = User.Identity.Name;
                model.DateCreated = DateTime.UtcNow;

                _invoiceRepo.Add(model);
                _invoiceRepo.SaveAll();
                await UploadFiles(files, model.InvoiceId);
                emailSubj = "New Invoice";
            }
            else
            {
                model.UpdatedBy = User.Identity.Name;
                model.DateUpdated = DateTime.UtcNow;

                _invoiceRepo.Update(model);
                _invoiceRepo.SaveAll();
                await UploadFiles(files, model.InvoiceId);
                emailSubj = "Invoice Update";
            }
                //Send Email
                string body = this.createEmailBody(model);
                await _emailSender.SendEmailAsync("", emailSubj, body);

                return RedirectToAction("index", "invoice", new { area = "" });                 
        }

        private async Task UploadFiles(List<IFormFile> files, int InvoiceId)
        {
            string folderName = "Upload";
            string webRootPath = _env.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            //create folder for invoiceId
            string invoiceFilePath = string.Format("{0}/{1}", newPath, InvoiceId);


            if (files != null && files.Count > 0)
            {

                if (!Directory.Exists(invoiceFilePath))
                    Directory.CreateDirectory(invoiceFilePath);

                foreach (IFormFile item in files)
                {
                    if (item.Length > 0)
                    {
                        //string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                        var fileName = Path.GetFileName(item.FileName);
                        string fullPath = Path.Combine(invoiceFilePath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            stream.Position = 0;
                            await item.CopyToAsync(stream);
                            //item.CopyTo(stream);
                        }
                    }
                }
            }
        }
        public JsonResult GetUploadedFiles()
        {
            int invoiceId = _inv.InvoiceId;
            if (invoiceId > 0)
            {
                string folderName = "Upload";
                string webRootPath = _env.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                //create folder for invoiceId
                string path = string.Format("{0}/{1}", newPath, invoiceId);
                var files = Directory.GetFiles(path).Select(file =>
                    new KendoTreeViewViewModel
                    {
                        Id = file,
                        HasChildren = false,
                        Name = Path.GetFileName(file)
                    });

                var directories = Directory.GetDirectories(path).Select(dir =>
                    new KendoTreeViewViewModel
                    {
                        Id = dir,
                        HasChildren = true,
                        Name = Path.GetFileName(dir)
                    });

                var result = files.ToList();
                result.AddRange(directories);
                result = result.OrderBy(x => x.HasChildren).ToList();

                return this.Json(result);
            }
            else return Json(new object[] { new object() });
        }

        public bool RemoveAllFiles(int invoiceId)
        {
            bool success = true;
            if (invoiceId > 0)
            {
                string folderName = "Upload";
                string webRootPath = _env.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                //create folder for invoiceId
                string path = string.Format(@"{0}\{1}", newPath, invoiceId);
                if (System.IO.Directory.Exists(path))
                {
                    foreach (var file in System.IO.Directory.GetFiles(path))
                    {
                        var filepath = Path.GetFullPath(file);
                        if (System.IO.File.Exists(filepath))
                            System.IO.File.Delete(filepath);

                    }
                    System.IO.Directory.Delete(path);
                }
            }

            return success;
        }

        #region Handling Invoice Grid
        public IActionResult Invoice_Read([DataSourceRequest] DataSourceRequest request)
        {
            var invoices = _invoiceRepo.GetAll()
                .Select(s => new
                {
                    InvoiceId= s.InvoiceId,
                    InvoiceName=s.InvoiceName,
                    InvoiceDate=s.InvoiceDate.ToString(),
                    CustomerName=string.Format("{0} {1}", s.FirstName, s.LastName),
                    Phone = s.Phone.ToString(),
                    Email = s.Email.ToString(),
                    WorkNotes=s.WorkNotes
                })
                .ToList();
            //List<InvoiceViewModel> ivm = new List<InvoiceViewModel>();
            //foreach(Invoice invoice in invoiceCoupons)
            //{

            //}
            return Json(invoices.ToDataSourceResult(request));
        }
        #endregion

        //[HttpGet, Route("GetStates")]
        public IActionResult GetStates()
        {
            var states = new[]
            {
                new State
                {
                    StateId=1,
                    StateName="CA"
                },
                new State
                {
                    StateId = 2,
                    StateName = "NV"
                }
            };
            return Json(states);
        }

        [HttpDelete, Route("invoice_delete")]
        public async Task<IActionResult> DeleteInvoiceAsync(int invoiceId)
        {
            string emailSubj = string.Empty;
            if (invoiceId > 0)
            {
                var invoice = _invoiceRepo.FindById(invoiceId);
                _invoiceRepo.Remove(invoice);
                _invoiceRepo.SaveAll();
                RemoveAllFiles(invoice.InvoiceId);
                emailSubj = string.Format("Invoice Name {0}, Invoice number {1} has been removed by {2} on {3}", invoice.InvoiceName, invoice.InvoiceId, User.Identity.Name, DateTime.UtcNow);
                string body = this.createEmailBody(invoice);
                await _emailSender.SendEmailAsync("", emailSubj, body);
            }

            return RedirectToAction("index", "invoice", new { area = "" });
        }

        public bool RemoveFile(int invoiceId,string fileName)
        {
            if (invoiceId > 0 && !string.IsNullOrEmpty(fileName))
            {
                string folderName = "Upload";
                string webRootPath = _env.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                //create folder for invoiceId
                string path = string.Format(@"{0}\{1}", newPath, invoiceId);
                if (System.IO.Directory.Exists(path))
                {
                    foreach (var file in System.IO.Directory.GetFiles(path))
                    {
                        var filepath = Path.GetFullPath(file);
                        if (System.IO.File.Exists(filepath) && fileName== Path.GetFileName(file))
                            System.IO.File.Delete(filepath);

                    }
                    bool anyFilesLeft = false;
                    foreach (var file in System.IO.Directory.GetFiles(path))
                    {
                        var filepath = Path.GetFullPath(file);
                        if (System.IO.File.Exists(filepath))
                            anyFilesLeft = true;

                    }
                    if (!anyFilesLeft) System.IO.Directory.Delete(path);
                }

            }
            return true;
        }

        private string createEmailBody(Invoice model)
        {

            string body = string.Empty;

            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "Invoice.html";

            body = System.IO.File.ReadAllText(pathToFile);

            body = body.Replace("{InvoiceId}", model.InvoiceId.ToString());
            body = body.Replace("{InvoiceDate}", model.InvoiceDate.ToShortDateString());
            body = body.Replace("{InvoiceName}", model.InvoiceName);
            body = body.Replace("{FirstName}", model.FirstName); //replacing the required things  
            body = body.Replace("{LastName}", model.LastName);
            body = body.Replace("{Phone}", model.Phone);
            body = body.Replace("{Email}", model.Email);
            body = body.Replace("{WorkNotes}", model.WorkNotes);
            return body;

        }
    }
}
