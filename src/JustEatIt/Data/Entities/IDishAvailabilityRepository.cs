using System.Collections.Generic;
using System.Linq;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IDishAvailabilityRepository
    {
        IQueryable<DishAvailability> GetAll { get; }

        int Save(DishAvailability dishAvail);

        void Update(DishAvailability dishAvailability);

        DishAvailability Delete(int dishAvailId);
    }
}