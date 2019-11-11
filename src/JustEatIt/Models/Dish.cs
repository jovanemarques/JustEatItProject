using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        
        [BindNever]
        [Required]
        public string Image { get; set; }

        [NotMapped]
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public DishType Type { get; set; }

        [Required]
        public Partner Partner { get; set; }
    }
}