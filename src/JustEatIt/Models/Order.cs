using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public class Order
    {
        [BindNever]
        public long Id { get; set; }

        [BindNever]
        public ICollection<OrderItem> Items { get; set; }

        [Required]
        public int Status { get; set; }

        public Customer Customer { get; set; }
    }
}
