using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public class EFItemOrderRepository : IItemOrderRepository
    {
        private readonly AppDataDbContext _context;

        public EFItemOrderRepository(AppDataDbContext context)
        {
            _context = context;
        }

        public int Create(OrderItem orderItem)
        {
            var newOrder = _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
            orderItem = newOrder.Entity;

            return orderItem.Id;
        }

        public int Update(OrderItem orderItem)
        {
            return 0;
        }
    }
}