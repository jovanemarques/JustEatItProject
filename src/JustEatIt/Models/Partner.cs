﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustEatIt.Models
{
    public class Partner
    {
        public string Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.001, 99.999)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rate { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }
        
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        
        [RegularExpression(@"^(?!.*[DFIOQU])[A-VXY][0-9][A-Z] ?[0-9][A-Z][0-9]$")]
        [StringLength(7)]
        [Required]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Range(-180, +180)]
        [Column(TypeName = "decimal(9, 6)")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Required]
        public decimal Longitude { get; set; }

        [Range(-90, 90)]
        [Column(TypeName = "decimal(8, 6)")]
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true, NullDisplayText = "")]
        [Required]
        public decimal Latitude { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}