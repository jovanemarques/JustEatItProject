using System.Collections.Generic;
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
        [Display(Name="Type")]
        [Range(1, int.MaxValue, ErrorMessage = "Type field is required.")]
        public int TypeId { get; set; }

        public DishType Type { get; set; }

        [Required]
        [ForeignKey("Partner")]
        [Display(Name = "Partner")]
        public string PartnerId { get; set; }

        public virtual Partner Partner { get; set; }

        public virtual  IEnumerable<DishAvailability> DishAvailabilities { get; set; }
    }
}