using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace JustEatIt.Models
{
    public class Dish
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name{ get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [NotMapped]
        [Required]
        public IFormFile File { get; set; }

        [ForeignKey("TypeId")]
        public int TypeId { get; set; }

        [Required]
        public DishType Type { get; set; }

        [ForeignKey("Partner")]
        public string PartnerId { get; set; }

        [Required]
        public virtual Partner Partner { get; set; }

        public virtual  DishAvailability DishAvailability { get; set; }
    }
}