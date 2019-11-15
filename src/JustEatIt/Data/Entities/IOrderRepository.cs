using System.Collections.Generic;
using JustEatIt.Models;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetAll { get; }

        int Create(Order order);

        Order GetOrderById(int id);

        bool UpdateStatus(long orderId, int status);

        IEnumerable<Order> GetOrdersForCustomer(string customerId);
    }
}