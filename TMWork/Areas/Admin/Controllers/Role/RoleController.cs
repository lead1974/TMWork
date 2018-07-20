﻿using TMWork.Areas.Admin.ViewModels.Role;
using TMWork.Data;
using TMWork.Data.Models.User;
using TMWork.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;


namespace TMWork.Areas.Admin.Controllers.Role
{
    [Area("Admin")]    
    [Route("admin/[controller]")]
    [Authorize(Roles = RoleName.CanManageSite)]
    public class RoleController:Controller
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly RoleManager<AuthRole> _roleManager;
        private readonly IMailService _emailSender;
        private readonly ISmsService _smsSender;
        private readonly ILogger _logger;

        private TMDbContext _TMDbContext;

        public RoleController(
            TMDbContext tmContext,
            UserManager<AuthUser> userManager,
            SignInManager<AuthUser> signInManager,
            RoleManager<AuthRole> roleManager,
            IMailService emailSender,
            ISmsService smsSender,
            ILoggerFactory loggerFactory)
        {
            _TMDbContext = tmContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<RoleController>();
        }

        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        public IActionResult EditingPopup_Read([DataSourceRequest] DataSourceRequest request)
        {
            var roles = _TMDbContext.Roles.Select(r => new RoleEdit
            {
                 Id = r.Id,
                 Name = r.Name,
                 Description = r.Description
            });

            return Json(roles.ToList().ToDataSourceResult(request));
        }

        [HttpPost, Route("create")]
        public ActionResult EditingPopup_Create([DataSourceRequest] DataSourceRequest request, RoleEdit theRole)
        {
            if (theRole != null && ModelState.IsValid)
            {
                if (!_roleManager.RoleExistsAsync(theRole.Name).Result)
                {
                    AuthRole newRole = new Data.Models.User.AuthRole();
                    newRole.Name = theRole.Name;
                    newRole.Description = theRole.Description;
                    IdentityResult roleResult = _roleManager.CreateAsync(newRole).Result;
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Error while creating role: " + roleResult.Errors.ToList());
                    }
                    return Json(new[] { newRole }.ToDataSourceResult(request, ModelState));
                }
                else { return BadRequest(); }                
            }
            return BadRequest();
        }

        [HttpPost, Route("edit")]
        public async Task<ActionResult> EditingPopup_Update([DataSourceRequest] DataSourceRequest request, RoleEdit theRole)
        {
            if (theRole != null && ModelState.IsValid)
            {
                AuthRole newRole = await _roleManager.FindByIdAsync(theRole.Id);                
                if (newRole == null) return BadRequest();

                newRole.Name = theRole.Name;
                newRole.Description = theRole.Description;

                await _roleManager.UpdateAsync(newRole);

                return Json(new[] { newRole }.ToDataSourceResult(request, ModelState));
            }

            return BadRequest();
        }

        // REMOVE api/values/5
        [HttpPost, Route("remove")]
        public async Task<IActionResult> EditingPopup_Destroy([DataSourceRequest] DataSourceRequest request, RoleEdit theRole)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(theRole.Id);
                if (role == null) return BadRequest("Role to remove not found!");

                //Remove All Users from current Role
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                if (users != null && users.Count>0)
                {
                    foreach (var u in users)
                    {
                        await _userManager.RemoveFromRoleAsync(u, role.Name);
                    }
                }

                //await _roleManager.DeleteAsync(role);
                return Ok();
            }

            return BadRequest(ModelState);
        }


    }
}
