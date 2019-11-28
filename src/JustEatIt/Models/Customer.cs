using System.ComponentModel.DataAnnotations;

namespace JustEatIt.Models
{
    public class Customer
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [RegularExpression(@"^(?!.*[DFIOQU])[A-VXY][0-9][A-Z] ?[0-9][A-Z][0-9]$")]
        [StringLength(7)]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        public string FullName => (LastName?.Trim()?.Length > 0) ? $"{FirstName.Trim()} {LastName.Trim()}" : FirstName.Trim();
    }
}
