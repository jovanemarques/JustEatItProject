using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public class OrderItem
    {
        public long Id { get; set; }

        public DishAvailability DishAvail { get; set; }

        public int Quantity { get; set; }
    }
}
