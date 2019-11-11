using JustEatIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    interface IOrderRepository
    {
        IQueryable<Order> GetAll { get; }
        long Save(Order order);
        bool UpdateStatus(long orderId, int status);
    }
}
