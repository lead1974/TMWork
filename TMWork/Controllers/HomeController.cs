using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using TMWork.Models;
using TMWork.Data.Models.User;
using TMWork.Services;

using TMWork.Data;
using TMWork.Data.Repos;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using TMWork.ViewModels.Home;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using TMWork.Data.Models.Customer;
using AutoMapper;

namespace TMWork.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _config;
        private readonly IHostingEnvironment _env;
        private TMDbContext _TMDbContext;
        private ICustomerCouponRepository _customerCouponRepo;
        private IContactRepository _contactRepo;

        public HomeController(
            TMDbContext dbContext,
            ICustomerCouponRepository customerCouponRepo,
            IContactRepository contactRepo,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory,
            IConfigurationRoot config,
            IHostingEnvironment env)
        {
            _TMDbContext = dbContext;
            _customerCouponRepo = customerCouponRepo;
            _contactRepo = contactRepo;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<HomeController>();
            _config = config;
            _env = env;
        }
        [SelectedTabFilter("home")]
        public IActionResult Index()
        {
            ViewBag.SelectiveTab = "home";

            var customerCoupons = _customerCouponRepo.GetAllNonExpired().ToList();
            ViewBag.ShowCoupons = "no";
            if (customerCoupons.Count > 0) ViewBag.ShowCoupons = "yes";

            return View();
        }
        [SelectedTabFilter("about")]
        public IActionResult About()
        {
            ViewBag.SelectiveTab = "about";
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [SelectedTabFilter("contact")]
        public IActionResult Contact()
        {
            ViewBag.SelectiveTab = "contact";
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                if (messages.ToLower().Contains("recaptcha"))
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", "Are you a human?");
                }
                return View(model);
            }
                //if (model.Email.Contains("aol.com"))
                //{
                //    ModelState.AddModelError("", "We don't support AOL addresses");
                //}
                    var contact = Mapper.Map<ContactViewModel,Contact>(model);
                    string body = this.createEmailBody(contact); 
                    await _emailSender.SendEmailAsync("", "from Contact page", body);
                    ModelState.Clear(); // Clear the form
                    ViewBag.UserMessage = string.Format("Dear {0},{1} We appreciate you contacting us.{1} One of our colleagues will get back to you shortly.",model.Name,"\n"); //Notify Users

                    //Insert Contact Information
                        var newContact = Mapper.Map<Contact>(model);

                        newContact.CreatedBy = (model.Name==null)?"Anonymous": model.Name;
                        newContact.DateCreated = DateTime.UtcNow;

                        newContact.UpdatedBy = (model.Name == null) ? "Anonymous" : model.Name;
                        newContact.DateUpdated = DateTime.UtcNow;

                        _contactRepo.Add(newContact);
                        _contactRepo.SaveAll();

            return View();
        }

        public IActionResult ContactEditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            var contacts = _TMDbContext.Contacts.Select(c => new Contact
            {
                ContactId = c.ContactId,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Message = c.Message,
                DateCreated = c.DateCreated,
                DateUpdated = c.DateUpdated,
                UpdatedBy=c.UpdatedBy
            }).OrderByDescending(c => c.ContactId);

            return Json(contacts.ToList().ToDataSourceResult(request));
        }

        [HttpPost, Route("ContactEditingPopup_Update")]
        public IActionResult ContactEditingPopup_Update([DataSourceRequest] DataSourceRequest request, Contact contact)
        {
            if (contact != null && ModelState.IsValid)
            {
                var theContact = contact; // Mapper.Map<Data.Models.Customer.Contact>(contact);

                theContact.UpdatedBy = User.Identity.Name;
                theContact.DateUpdated = DateTime.UtcNow;

                _contactRepo.Update(theContact);
                _contactRepo.SaveAll();
                return Json(new[] { theContact }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        [HttpPost, Route("ContactEditingPopup_Destroy")]
        public IActionResult ContactEditingPopup_Destroy([DataSourceRequest] DataSourceRequest request, Contact contact)
        {
            if (contact != null)
            {
                var theContact = contact; // Mapper.Map<Data.Models.Customer.Contact>(theContact);

                _contactRepo.Remove(_contactRepo.FindById(theContact.ContactId));
                _contactRepo.SaveAll();
                return Json(new[] { theContact }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        //Get Coupons 
        public IActionResult GetCustomerCoupons([DataSourceRequest] DataSourceRequest request)
        {
            var customerCoupons = _customerCouponRepo.GetAllNonExpired().ToList();
            return Json(customerCoupons.ToDataSourceResult(request));
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string createEmailBody(Contact model)
        {

            string body = string.Empty;

            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "ContactUs.html";

            body = System.IO.File.ReadAllText(pathToFile); 

            body = body.Replace("{Name}", model.Name); //replacing the required things  
            body = body.Replace("{Phone}", model.Phone);
            body = body.Replace("{Email}", model.Email);
            body = body.Replace("{Message}", model.Message);
            return body;

        }
    }
}
