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
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly ICustomerRepository _repoCust;
        
        public IndexModel(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            ICustomerRepository repoCust)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repoCust = repoCust;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(CustomUser user, Customer customer)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Customer = customer;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Partner"))
            {
                return RedirectToPage("./IndexPartner");
            }
            else if (User.IsInRole("Administrator"))
            {
                return RedirectToPage("./IndexAdmin");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Customer customer = _repoCust.GetAll.FirstOrDefault(c => c.Id == user.Id);
            if (customer == null)
            {
                customer = new Customer{ Id = user.Id } ;
            }

            await LoadAsync(user, customer);
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
                await LoadAsync(user, Customer);
                return Page();
            }

            if ((Input.PhoneNumber != user.PhoneNumber) || (Customer.FirstName != user.Name))
            {
                user.PhoneNumber = Input.PhoneNumber;
                user.Name = Customer.FirstName;

                var setResult = await _userManager.UpdateAsync(user);
                if (!setResult.Succeeded)
                { 
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (_repoCust.Save(Customer) == null)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred saving information for the customer with ID '{userId}'.");
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
