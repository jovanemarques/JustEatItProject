using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
