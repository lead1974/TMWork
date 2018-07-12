using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMWork.Data;
using TMWork.Data.Models.Customer;
using TMWork.Data.Models.User;
using TMWork.Data.Repos;
using TMWork.Services;

namespace TMWork.Controllers.Api
{
    [Route("api/[controller]")]
    public class ContactApiController: Controller
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
        private IContactRepository _contactRepo;

        public ContactApiController(
            TMDbContext dbContext,
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

        [HttpGet]
        public IActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var contacts = _TMDbContext.Contacts.Select(c => new Contact
            {
                ContactId = c.ContactId,
                Name = c.Name,
                Phone = c.Phone,
                Email = c.Email,
                Message = c.Message,
                DateCreated=c.DateCreated
            }).OrderByDescending(c => c.ContactId);

            return Json(contacts.ToList().ToDataSourceResult(request));
        }
    }
}
