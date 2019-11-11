using JustEatIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data
{
    interface IDishAvailabilityRepository
    {
        IQueryable<DishAvailability> GetAll { get; }

        long Save(DishAvailability dishAvail);

        DishAvailability Delete(int dishAvailId);
    }
}
