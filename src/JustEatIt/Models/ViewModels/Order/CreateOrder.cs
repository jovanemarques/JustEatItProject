using System.Collections.Generic;

namespace JustEatIt.Models.ViewModels.Order
{
    public class CreateOrder
    {
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}