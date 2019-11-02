using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace JustEatIt.Models
{
    public class Dish
    {
        public int Id { get; set; }

        public string Name{ get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public int Quantity{ get; set; }

        public DateTime BestBefore { get; set; }

        public string Image { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }

        public string Type { get; set; }

        public string Restaurant { get; set; }

        public Partner Partner { get; set; }
    }
}