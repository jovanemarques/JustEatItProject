using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustEatIt.Models
{
    public class DishAvailability
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Available from")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Expire date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Original price")]
        [Column(TypeName = "decimal(11, 2)")]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Display(Name = "Discount price")]
        [Column(TypeName = "decimal(11, 2)")]
        public decimal DiscountPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public virtual Dish Dish { get; set; }
    }
}