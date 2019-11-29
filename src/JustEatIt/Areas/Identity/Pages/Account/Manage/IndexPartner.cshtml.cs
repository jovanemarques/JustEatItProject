using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustEatIt.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexPartnerModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IPartnerRepository _repoPart;
        
        public IndexPartnerModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IPartnerRepository repoPart)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoPart = repoPart;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Partner Partner { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(CustomUser user, Partner partner)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Partner = partner;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole("Partner"))
            {
                return NotFound($"User doesn't belong to this role '{_userManager.GetUserId(User)}'.");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Partner partner = _repoPart.GetAll.FirstOrDefault(c => c.Id == user.Id);
            if (partner == null)
            {
                partner = new Partner { Id = user.Id } ;
            }

            await LoadAsync(user, partner);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user, Partner);
                return Page();
            }

            if ((Input.PhoneNumber != user.PhoneNumber) || (Partner.Name != user.Name))
            {
                user.PhoneNumber = Input.PhoneNumber;
                user.Name = Partner.Name;

                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (_repoPart.Save(Partner) == null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred saving information for the partner with ID '{userId}'.");
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
