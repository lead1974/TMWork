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
using TMWork.Data.Models.Team;
using System.Web;
using Microsoft.AspNetCore.Http;

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
        private IMissionRepository _missionRepo;
        private IMemberRepository _memberRepo;

        [BindProperty]
        public IFormFile Image { get; set; }

        public HomeController(
            TMDbContext dbContext,
            ICustomerCouponRepository customerCouponRepo,
            IContactRepository contactRepo,
            IMissionRepository missionRepo,
            IMemberRepository memberRepo,
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
            _missionRepo = missionRepo;
            _memberRepo = memberRepo;
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
        #region About
        [SelectedTabFilter("about")]
        public IActionResult About()
        {
            var mission = _missionRepo.GetAll().ToList().FirstOrDefault();
            ViewBag.OurMission = HttpUtility.HtmlDecode(mission.OurMission);
            ViewBag.SelectiveTab = "about";
            ViewData["Message"] = "Your application description page.";

            //Get Team Members
            var members = _memberRepo.GetAll().ToList();
            foreach(var m in members)
            {
                m.Description = HttpUtility.HtmlDecode(m.Description);
            }
            ViewBag.Members = members;

            return View();
        }

        #region OurMission
        [HttpGet, Route("AboutEditOurMission")]
        public IActionResult AboutEditOurMission()
        {
            var mission = _missionRepo.GetAll().ToList().FirstOrDefault();
            mission.OurMission = HttpUtility.HtmlDecode(mission.OurMission);
            ViewBag.SelectiveTab = "about";
            return View("AboutEditOurMission", mission);
        }
        [HttpPost, Route("AboutEditOurMission")]
        [ValidateAntiForgeryToken]
        public IActionResult AboutEditOurMission(Mission model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var emailSubj = string.Empty;
            if (model.MissionId == 0)
            {
                model.CreatedBy = User.Identity.Name;
                model.DateCreated = DateTime.UtcNow;

                _missionRepo.Add(model);
                _missionRepo.SaveAll();
            }
            else
            {
                model.UpdatedBy = User.Identity.Name;
                model.DateUpdated = DateTime.UtcNow;

                _missionRepo.Update(model);
                _missionRepo.SaveAll();
            }

            return RedirectToAction("About", "Home");
        }
        #endregion

        #region TeamMembers
        [HttpGet, Route("AboutGetTeamMember/{memberId?}")]
        public IActionResult AboutGetTeamMember(int memberId)
        {
            ViewBag.SelectiveTab = "about";
            
            if (memberId > 0)
            {
                var member = _memberRepo.FindById(memberId);
                member.Description = HttpUtility.HtmlDecode(member.Description);
                return View("AboutEditTeamMember", member);
            }
            return View("AboutEditTeamMember", new Member());
        }

        [HttpPost, Route("AboutEditTeamMember")]
        [ValidateAntiForgeryToken]
        public IActionResult AboutEditTeamMember(Member model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (Image != null)
            {
                using (var stream = new System.IO.MemoryStream())
                {
                    Image.CopyTo(stream);
                    model.Image = stream.ToArray();
                    model.ImageContentType = Image.ContentType;
                }
            }
            else 
            {
                var m = _memberRepo.FindById(model.MemberId);
                model.Image = m.Image;
                model.ImageContentType = m.ImageContentType;
            }
            

            model.Name = model.Name.Trim();
            var emailSubj = string.Empty;
            if (model.MemberId == 0)
            {
                model.CreatedBy = User.Identity.Name;
                model.DateCreated = DateTime.UtcNow;

                _memberRepo.Add(model);
            }
            else
            {
                model.UpdatedBy = User.Identity.Name;
                model.DateUpdated = DateTime.UtcNow;

                _memberRepo.Update(model);                
            }

            _memberRepo.SaveAll();
            return RedirectToAction("About", "Home");
        }
        //Get Members 
        public IActionResult GetMembers([DataSourceRequest] DataSourceRequest request)
        {
            var members = _memberRepo.GetAll().ToList();
            return Json(members.ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeamMemberAsync(int memberId)
        {
            string emailSubj = string.Empty;
            if (memberId > 0)
            {
                var member = _memberRepo.FindById(memberId);
                _memberRepo.Remove(member);
                _memberRepo.SaveAll();
                emailSubj = string.Format("Member {0}, Member number {1} has been removed by {2} on {3}", member.Name, member.MemberId, User.Identity.Name, DateTime.UtcNow);
                string body = this.createEmailBody_Member(member, emailSubj);
                await _emailSender.SendEmailAsync("", emailSubj, body);
            }

            return RedirectToAction("About", "Home", new { area = "" });
        }
        #endregion TeamMember

        #endregion

        #region Contact
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
                    string body = this.createEmailBody_Contact(contact); 
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
        #endregion

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetOurMissionText()
        {

            string ourMission = string.Empty;

            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "TextFiles"
                    + Path.DirectorySeparatorChar.ToString()
                    + "OurMission.html";

            ourMission = System.IO.File.ReadAllText(pathToFile);
            return ourMission;        }

        private string createEmailBody_Contact(Contact model)
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
        private string createEmailBody_Member(Member model,string subj)
        {

            string body = string.Empty;

            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "templates"
                    + Path.DirectorySeparatorChar.ToString()
                    + "EmailTemplate"
                    + Path.DirectorySeparatorChar.ToString()
                    + "Member.html";

            body = System.IO.File.ReadAllText(pathToFile);

            body = body.Replace("{Name}", model.Name); //replacing the required things  
            body = body.Replace("{Id}", model.MemberId.ToString());
            body = body.Replace("{Message}", subj);
            return body;
        }
    }
}
