using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models.ViewModels
{
    public class AdminAdminViewModel
    {
        public string Username { get; set; }

        public Administrator Administrator { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string SearchText { get; set; }

        public string OldRole { get; set; }
    }
}
