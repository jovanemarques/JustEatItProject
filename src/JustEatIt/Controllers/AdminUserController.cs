using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository _repoUser;
        private readonly ICustomerRepository _repoCust;
        private readonly IPartnerRepository _repoPart;
        private readonly UserManager<CustomUser> _userManager;

        public AdminUserController(IUserRepository repoUser, UserManager<CustomUser> userManager,
            ICustomerRepository repoCust, IPartnerRepository repoPart)
        {
            _repoUser = repoUser;
            _userManager = userManager;
            _repoCust = repoCust;
            _repoPart = repoPart;
        }
        
        public IActionResult Index(string searchText)
        {
            IEnumerable<UserInfo> users;
            ViewData["currentFilter"] = searchText;

            if ((searchText != null) && (searchText.Trim().Length > 0))
            {
                users = _repoUser.GetAll.Where(d => d.Name.Contains(searchText)
                    || d.Role.Contains(searchText)
                    || d.UserName.Contains(searchText));
            }
            else
            {
                users = _repoUser.GetAll;
            }

            return View(users);
        }

        public IActionResult EditUser(string id, string role, string searchText)
        {
            switch (role?.Trim())
            {
                case "":
                case null:
                case "Customer":
                    return RedirectToAction(nameof(EditCustomer), new { 
                        id = id,
                        searchText = searchText });

                case "Partner":
                    return RedirectToAction(nameof(EditPartner), new
                    {
                        id = id,
                        searchText = searchText
                    });

                case "Administrator":
                    return RedirectToAction(nameof(EditAdmin), new
                    {
                        id = id,
                        searchText = searchText
                    });

                default:
                    TempData["StatusMessage"] = "#E#:Unable to find the User.";
                    break;
            }

            return RedirectToAction(nameof(Index), new { searchText = searchText });
        }

        public async Task<IActionResult> EditCustomer(string id, string searchText)
        {
            ViewData["currentFilter"] = searchText;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            if (! await _userManager.IsInRoleAsync(user, "Customer"))
            {
                if (!await _userManager.IsInRoleAsync(user, "Administrator") && !await _userManager.IsInRoleAsync(user, "Partner"))
                {
                    await _userManager.AddToRoleAsync(user, "Customer");                    
                }
                else
                {
                    TempData["StatusMessage"] = "#E#:The user do not belong to this role.";
                    return RedirectToAction(nameof(Index), new { searchText = searchText });
                }
            }

            var customer = _repoCust.GetAll.FirstOrDefault(c => c.Id == id);
            if ((customer == null))
            {
                customer = new Customer() { Id = user.Id };
            }

            return View(new AdminCustomerViewModel()
                {
                    Username = user.UserName,
                    Customer = customer,
                    PhoneNumber = user.PhoneNumber,
                    SearchText = searchText
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(AdminCustomerViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Customer.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{model.Customer.Id}'.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if ((model.PhoneNumber != user.PhoneNumber) || (model.Customer.FullName != user.Name))
            {
                user.PhoneNumber = model.PhoneNumber;
                user.Name = model.Customer.FullName;

                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                {
                    TempData["StatusMessage"] = "#E#:Unable to update the User information.";
                    return View(model);
                }
            }

            if ((model.OldRole == "Partner") || (model.OldRole == "Administrator") || model?.OldRole == "None")
            {
                await SetupNewRole(user, model.OldRole, "Customer");
            }

            if (_repoCust.Save(model.Customer) == null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                TempData["StatusMessage"] = $"Unexpected error occurred saving information for the customer with ID '{userId}'.";
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { searchText = model.SearchText });
        }

        private async Task SetupNewRole(CustomUser user, string oldRole, string newRole)
        {
            if (oldRole != "None")
            {
                if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    _repoCust.DeleteAll(user.Id); ;
                    await _userManager.RemoveFromRoleAsync(user, "Customer");
                }

                if (await _userManager.IsInRoleAsync(user, "Partner"))
                {
                    _repoPart.DeleteAll(user.Id); ;
                    await _userManager.RemoveFromRoleAsync(user, "Partner");
                }

                await _userManager.RemoveFromRoleAsync(user, "Administrator");
            }

            await _userManager.AddToRoleAsync(user, newRole);
        }

        public async Task<IActionResult> EditPartner(string id, string searchText)
        {
            ViewData["currentFilter"] = searchText;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            if (!await _userManager.IsInRoleAsync(user, "Partner"))
            {
                TempData["StatusMessage"] = "#E#:The user do not belong to this role.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            var partner = _repoPart.GetAll.FirstOrDefault(c => c.Id == id);
            if ((partner == null))
            {
                partner = new Partner() { Id = user.Id };
            }

            return View(new AdminPartnerViewModel()
            {
                Username = user.UserName,
                Partner = partner,
                PhoneNumber = user.PhoneNumber,
                SearchText = searchText
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPartner(AdminPartnerViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Partner.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{model.Partner.Id}'.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if ((model.PhoneNumber != user.PhoneNumber) || (model.Partner.Name != user.Name))
            {
                user.PhoneNumber = model.PhoneNumber;
                user.Name = model.Partner.Name;

                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                {
                    TempData["StatusMessage"] = "#E#:Unable to update the User information.";
                    return View(model);
                }
            }

            if ((model.OldRole == "Customer") || (model.OldRole == "Administrator") || model?.OldRole == "None")
            {
                await SetupNewRole(user, model.OldRole, "Partner");
            }

            if (_repoPart.Save(model.Partner) == null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                TempData["StatusMessage"] = $"Unexpected error occurred saving information for the partner with ID '{userId}'.";
                return View(model);
            }

            return RedirectToAction(nameof(Index), new { searchText = model.SearchText });
        }

        public async Task<IActionResult> EditAdmin(string id, string searchText)
        {
            ViewData["currentFilter"] = searchText;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                TempData["StatusMessage"] = "#E#:The user do not belong to this role.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            return View(new AdminAdminViewModel()
            {
                Username = user.UserName,
                Administrator = new Administrator { Id = user.Id, Name = user.Name },
                PhoneNumber = user.PhoneNumber,
                SearchText = searchText
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(AdminAdminViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Administrator.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{model.Username}'.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if ((model.PhoneNumber != user.PhoneNumber) || (model.Administrator.Name != user.Name))
            {
                user.PhoneNumber = model.PhoneNumber;
                user.Name = model.Administrator.Name;

                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                {
                    TempData["StatusMessage"] = "#E#:Unable to update the User information.";
                    return View(model);
                }
            }

            if ((model?.OldRole == "Partner") || (model?.OldRole == "Customer") || model?.OldRole == "None")
            {
                await SetupNewRole(user, model.OldRole, "Administrator");
            }

            return RedirectToAction(nameof(Index), new { searchText = model.SearchText });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, [Bind("SearchText")] string searchText)
        {
            ViewData["currentFilter"] = searchText;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
            }
            else
            {
                if (await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    _repoCust.DeleteAll(user.Id); ;
                }

                if (await _userManager.IsInRoleAsync(user, "Partner"))
                {
                    _repoPart.DeleteAll(user.Id); ;
                }

                await _userManager.DeleteAsync(user);

                TempData["StatusMessage"] = $"User '{user.UserName}' was deleted sucessfully.";
            }

            return RedirectToAction(nameof(Index), new { searchText = searchText });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove2FA(string id, [Bind("SearchText")] string searchText)
        {
            ViewData["currentFilter"] = searchText;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
            }
            else
            {
                var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
                if (!disable2faResult.Succeeded)
                {
                    TempData["StatusMessage"] = $"#E#:Unexpected error occurred disabling 2FA for user with ID '{_userManager.GetUserId(User)}'.";
                }
                else
                {
                    TempData["StatusMessage"] = "The Two-factor Authentication was removed for this user.";
                }
            }

            return RedirectToAction(nameof(Index), new { searchText = searchText });
        }

        public async Task<IActionResult> ChangeRole(string id, string newRole, string searchText)
        {
            ViewData["currentFilter"] = searchText;

            if (!newRole.Equals("Administrator") && !newRole.Equals("Partner") && !newRole.Equals("Customer"))
            {
                TempData["StatusMessage"] = "#E#:This role is not available.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the User.";
                return RedirectToAction(nameof(Index), new { searchText = searchText });
            }

            string oldRole;
            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                oldRole = "Administrator";
            }
            else if (await _userManager.IsInRoleAsync(user, "Partner"))
            {
                oldRole = "Partner";
            }
            else if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                oldRole = "Customer";
            }
            else
            {
                oldRole = "None";
            }
            
            switch (newRole)
            {
                case "Customer":
                    return View(nameof(EditCustomer), new AdminCustomerViewModel()
                        {
                            Username = user.UserName,
                            Customer = new Customer { Id = user.Id },
                            PhoneNumber = user.PhoneNumber,
                            SearchText = searchText,
                            OldRole = oldRole
                        });

                case "Partner":
                    return View(nameof(EditPartner), new AdminPartnerViewModel()
                        {
                            Username = user.UserName,
                            Partner = new Partner { Id = user.Id },
                            PhoneNumber = user.PhoneNumber,
                            SearchText = searchText,
                            OldRole = oldRole
                    });

                case "Administrator":
                    return View(nameof(EditAdmin), new AdminAdminViewModel()
                        {
                            Username = user.UserName,
                            Administrator = new Administrator { Id = user.Id },
                            PhoneNumber = user.PhoneNumber,
                            SearchText = searchText,
                            OldRole = oldRole
                    });
            }

            return RedirectToAction(nameof(Index), new { searchText = searchText });
        }


    }
}