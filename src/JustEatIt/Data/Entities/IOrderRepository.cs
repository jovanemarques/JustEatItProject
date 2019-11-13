using System.Collections.Generic;
using JustEatIt.Models;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetAll { get; }

        long Save(Order order);

        bool UpdateStatus(long orderId, int status);

        IEnumerable<Order> GetOrdersForCustomer(string customerId);
    }
}