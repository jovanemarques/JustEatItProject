using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly AppDataDbContext _context;
        private readonly IDishAvailabilityRepository _dishAvailabilityRepository;

        public EFOrderRepository(
            AppDataDbContext context,
            IDishAvailabilityRepository dishAvailabilityRepository)
        {
            _context = context;
            _dishAvailabilityRepository = dishAvailabilityRepository;
        }

        public IQueryable<Order> GetAll => _context.Orders
            .Include(c => c.Customer)
            .Include(i => i.Items)            
            .ThenInclude(da => da.DishAvail)
            .ThenInclude(d => d.Dish);

        public int Create(Order order)
        {
            _context.AttachRange(order.Items.Select(l => l.DishAvail));
            var newOrder = _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var orderItem in order.Items)
            {
                orderItem.DishAvail.Quantity -= orderItem.Quantity;
                _dishAvailabilityRepository.Update(orderItem.DishAvail);
            }

            return newOrder.Entity.Id;
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders.Include(x => x.Partner).Include(x => x.Items).ThenInclude(c => c.DishAvail.Dish).FirstOrDefault(order => order.Id == id);
        }

        public bool UpdateStatus(long orderId, int status)
        {
            bool result = false;

            Order dbOrder = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (dbOrder != null)
            {
                dbOrder.Status = status;
                result = true;
            }        

            _context.SaveChanges();
            return result;
        }

        public IEnumerable<Order> GetOrdersForCustomer(string customerId)
        {
            return _context.Orders.Where(order => order.Customer.Id.Equals(customerId))
                .Include(x => x.Partner)
                .Include(x => x.Items)
                .ThenInclude(da => da.DishAvail)
                .ToList();
        }
    }
}