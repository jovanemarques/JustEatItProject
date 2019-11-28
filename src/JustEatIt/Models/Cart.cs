using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public class Cart
    {
        private List<OrderItem> itemList = new List<OrderItem>();

        public virtual void AddItem(DishAvailability dishAvail, int quantity)
        {
            OrderItem line = itemList
                .Where(i => i.Id == dishAvail.Id)
                .FirstOrDefault();

            if (line == null)
            {
                itemList.Add(new OrderItem
                {
                    DishAvail = dishAvail,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public virtual void RemoveItem(DishAvailability dishAvail) =>
            itemList.RemoveAll(l => l.DishAvail.Id == dishAvail.Id);

        public virtual decimal ComputeTotalValue() =>
            itemList.Sum(e => e.DishAvail.DiscountPrice * e.Quantity);

        public virtual void Clear() => itemList.Clear();

        public virtual IEnumerable<OrderItem> Items => itemList;
    }
}
