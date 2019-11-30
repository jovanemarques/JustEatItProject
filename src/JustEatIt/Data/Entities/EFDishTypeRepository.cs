using JustEatIt.Models;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public class EFDishTypeRepository : IDishTypeRepository
    {
        private AppDataDbContext context;

        public EFDishTypeRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<DishType> GetAll => context.DishTypes;

        public int Save(DishType dishType)
        {
            DishType dbDishType;

            if (dishType.Id == 0)
            {
                var newDT = context.DishTypes.Add(dishType);
                context.SaveChanges();
                dbDishType = newDT.Entity;
            }
            else
            {
                dbDishType = context.DishTypes.FirstOrDefault(r => r.Id == dishType.Id);
                if (dbDishType != null)
                {
                    dbDishType.TypeName = dishType.TypeName;
                }
            }

            context.SaveChanges();
            return dbDishType.Id;
        }

        public DishType Delete(int id)
        {
            DishType dbDishType = context.DishTypes.FirstOrDefault(r => r.Id == id);
            if (dbDishType != null)
            {
                context.DishTypes.Remove(dbDishType);
                context.SaveChanges();
            }
            return dbDishType;
        }
    }
}
