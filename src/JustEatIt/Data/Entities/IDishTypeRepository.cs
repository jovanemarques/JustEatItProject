using JustEatIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public interface IDishTypeRepository
    {
        IQueryable<DishType> GetAll { get; }

        int Save(DishType dishType);

        DishType Delete(int id);

    }
}
