using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        
        [Display(Name = "Login Id")]
        public string UserName { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        [Display(Name = "2FA")]
        public bool Use2FA { get; set; }

        public bool Local { get; set; }

        public int ExternalLogins { get; set; }

        public DateTime LastAccess { get; set; }
    }
}
