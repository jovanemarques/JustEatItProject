using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JustEatIt.Data.Entities
{
    public class EFDishRepository : IDishRepository
    {
        private AppDataDbContext context;

        public EFDishRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Dish> GetAll => context.Dishes
            .Include(d => d.Type)
            .Include(d => d.Partner)
            .Include(d => d.DishAvailabilities);

        public int Save(Dish dish)
        {
            Dish dbDish;

            if (dish.Id == 0)
            {
                var newDish = context.Dishes.Add(dish);
                context.SaveChanges();
                dbDish = newDish.Entity;
            }
            else
            {
                dbDish = context.Dishes.FirstOrDefault(r => r.Id == dish.Id);
                if (dbDish != null)
                {
                    dbDish.Name = dish.Name;
                    dbDish.Description = dish.Description;
                    dbDish.Type = dish.Type;
                }
            }

            context.SaveChanges();
            return dbDish.Id;
        }

        public Dish Delete(int id)
        {
            Dish dbDish = context.Dishes.FirstOrDefault(r => r.Id == id);
            if (dbDish != null)
            {
                context.Dishes.Remove(dbDish);
                context.SaveChanges();
            }
            return dbDish;
        }
    }
}
