using System.Collections.Generic;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IDishAvailabilityRepository
    {
        IEnumerable<DishAvailability> GetAll();

        long Save(DishAvailability dishAvail);

        DishAvailability Delete(int dishAvailId);
    }
}