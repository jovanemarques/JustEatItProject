using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public UserManager<IdentityUser> UserManager { get; set; }

        public AdminController(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddRole(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    if (!await UserManager.IsInRoleAsync(user, model.Role))
                    {
                        var result = await UserManager.AddToRoleAsync(user, model.Role);
                        if (result.Succeeded)
                        {
                            ModelState.AddModelError("", "User added to the role successfully.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error adding user to the role.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Role", "User already belong to this role.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email not found.");
                }
            }
            return View("Index", model);
        }

        public async Task<IActionResult> RemoveRole(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    if (await UserManager.IsInRoleAsync(user, model.Role))
                    {
                        var result = await UserManager.RemoveFromRoleAsync(user, model.Role);
                        if (result.Succeeded)
                        {
                            ModelState.AddModelError("", "User removed from the role successfully.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error adding user to the role.");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("Role", "User do not belong to this role.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email not found.");
                }
            }
            return View("Index", model);
        }
    }
}