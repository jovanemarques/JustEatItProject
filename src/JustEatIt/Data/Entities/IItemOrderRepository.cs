using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IItemOrderRepository
    {
        int Create(OrderItem orderItem);

        int Update(OrderItem orderItem);
    }
}