using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustEatIt.Models
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }

        [BindNever]
        public ICollection<OrderItem> Items { get; set; }

        [Required]
        public int Status { get; set; }

        public Customer Customer { get; set; }
    }
}