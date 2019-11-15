using System.ComponentModel.DataAnnotations;

namespace JustEatIt.Models.ViewModels.Order
{
    public class CreditCard
    {
        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Expiration { get; set; }

        [Required]
        public int? Cvv { get; set; }
    }
}