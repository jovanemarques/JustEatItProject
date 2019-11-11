using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Data.Entities
{
    public class EFDishAvailabilityRepository : IDishAvailabilityRepository
    {
        private AppDataDbContext context;

        public EFDishAvailabilityRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<DishAvailability> GetAll => context.DishesAvail.Include(d => d.Dish);

        public long Save(DishAvailability dishAvail)
        {
            DishAvailability dbDishAvail;

            if (dishAvail.Id == 0)
            {
                var newDish = context.DishesAvail.Add(dishAvail);
                context.SaveChanges();
                dbDishAvail = newDish.Entity;
            }
            else
            {
                dbDishAvail = context.DishesAvail.FirstOrDefault(da => da.Id == dishAvail.Id);
                if (dbDishAvail != null)
                {
                    dbDishAvail.StartDate = dishAvail.StartDate;
                    dbDishAvail.EndDate = dishAvail.EndDate;
                    dbDishAvail.OriginalPrice = dishAvail.OriginalPrice;
                    dbDishAvail.DiscountPrice = dishAvail.DiscountPrice;
                    dbDishAvail.Quantity = dishAvail.Quantity;
                }
            }

            context.SaveChanges();
            return dbDishAvail.Id;
        }

        public DishAvailability Delete(int dishAvailId)
        {
            DishAvailability dbDishAvail = context.DishesAvail.FirstOrDefault(da => da.Id == dishAvailId);
            if (dbDishAvail != null)
            {
                context.DishesAvail.Remove(dbDishAvail);
                context.SaveChanges();
            }
            return dbDishAvail;
        }
    }
}
