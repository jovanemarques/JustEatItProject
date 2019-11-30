using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models.ViewModels
{
    public class AdminCustomerViewModel
    {
        public string Username { get; set; }

        public Customer Customer { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string SearchText { get; set; }

        public string OldRole { get; set; }
    }
}
