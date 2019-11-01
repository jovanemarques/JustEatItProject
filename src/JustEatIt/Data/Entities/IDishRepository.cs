using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public interface IDishRepository
    {
        IQueryable<Dish> GetAll { get; }
        Dish Save(Dish dish);
        Dish Delete(int dishId);
    }
}
