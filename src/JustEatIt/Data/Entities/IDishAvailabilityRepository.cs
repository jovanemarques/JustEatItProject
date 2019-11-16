using System.Collections.Generic;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IDishAvailabilityRepository
    {
        IEnumerable<DishAvailability> GetAll();

        int Save(DishAvailability dishAvail);

        void Update(DishAvailability dishAvailability);

        DishAvailability Delete(int dishAvailId);
    }
}