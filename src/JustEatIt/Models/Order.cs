using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustEatIt.Models
{
    public class Order
    {
        [BindNever]
        public int Id { get; set; }

        [BindNever]
        public virtual ICollection<OrderItem> Items { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }

        [Display(Name = "Date")]
        public DateTime OrderedAt { get; set; }

        public virtual Customer Customer { get; set; }

        [Required]
        [ForeignKey("Partner")]
        public string PartnerId { get; set; }

        public virtual Partner Partner { get; set; }
        
        public decimal GetTotal()
        {
            decimal result = 0;
            foreach (var orderItem in Items)
            {
                result += orderItem.DishAvail.DiscountPrice * orderItem.Quantity;
            }

            return result;
        }

        public string GetOrderStatus()
        {
            return Enum.GetName(typeof(OrderStatus), Status);
        }
    }
}