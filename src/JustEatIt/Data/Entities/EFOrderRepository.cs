using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFOrderRepository : IOrderRepository
    {
        private AppDataDbContext context;

        public EFOrderRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Order> GetAll => context.Orders
            .Include(i => i.Items)
            .ThenInclude(da => da.DishAvail)
            .ThenInclude(d => d.Dish);

        public long Save(Order order)
        {
            if (order.Id == 0)
            {
                context.AttachRange(order.Items.Select(l => l.DishAvail));
                var newOrder = context.Orders.Add(order);
                context.SaveChanges();
                order = newOrder.Entity;
            }

            context.SaveChanges();
            return order.Id;
        }

        public bool UpdateStatus(long orderId, int status)
        {
            bool result = false;

            Order dbOrder = context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (dbOrder != null)
            {
                dbOrder.Status = status;
                result = true;
            }        

            context.SaveChanges();
            return result;
        }
    }
}
