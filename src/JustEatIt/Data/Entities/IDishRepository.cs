using System.Linq;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IDishRepository
    {
        IQueryable<Dish> GetAll { get; }

        int Save(Dish dish);

        Dish Delete(int dishId);
    }
}