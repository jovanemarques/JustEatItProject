using System.ComponentModel.DataAnnotations;

namespace JustEatIt.Models
{
    public class DishType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Type")]
        public string TypeName { get; set; }
    }
}
