using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string Type { get; set; }
        public string Restaurant { get; set; }
        public Partner Partner { get; set; }
    }
}
