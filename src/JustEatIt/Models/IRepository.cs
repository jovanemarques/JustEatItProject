using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public interface IRepository
    {
        IQueryable<Dish> Dishes { get; }
    }
}
