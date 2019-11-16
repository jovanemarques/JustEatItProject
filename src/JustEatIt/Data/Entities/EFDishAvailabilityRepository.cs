using System.Collections.Generic;
using System.Linq;
using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Data.Entities
{
    public class EFDishAvailabilityRepository : IDishAvailabilityRepository
    {
        private readonly AppDataDbContext _context;

        public EFDishAvailabilityRepository(AppDataDbContext context)
        {
            _context = context;
        }

        public IQueryable<DishAvailability> GetAll => _context.DishesAvail.Include(d => d.Dish);

        public int Save(DishAvailability dishAvail)
        {
            DishAvailability dbDishAvail;

            if (dishAvail.Id == 0)
            {
                var newDish = _context.DishesAvail.Add(dishAvail);
                _context.SaveChanges();
                dbDishAvail = newDish.Entity;
            }
            else
            {
                dbDishAvail = _context.DishesAvail.FirstOrDefault(da => da.Id == dishAvail.Id);
                if (dbDishAvail != null)
                {
                    dbDishAvail.StartDate = dishAvail.StartDate;
                    dbDishAvail.EndDate = dishAvail.EndDate;
                    dbDishAvail.OriginalPrice = dishAvail.OriginalPrice;
                    dbDishAvail.DiscountPrice = dishAvail.DiscountPrice;
                    dbDishAvail.Quantity = dishAvail.Quantity;
                }
            }

            _context.SaveChanges();

            return dbDishAvail.Id;
        }

        public void Update(DishAvailability dishAvailability)
        {
            var existingDishAvailability = _context.DishesAvail.FirstOrDefault(da => da.Id == dishAvailability.Id);
            if (existingDishAvailability != null)
            {
                existingDishAvailability.StartDate = dishAvailability.StartDate;
                existingDishAvailability.EndDate = dishAvailability.EndDate;
                existingDishAvailability.OriginalPrice = dishAvailability.OriginalPrice;
                existingDishAvailability.DiscountPrice = dishAvailability.DiscountPrice;
                existingDishAvailability.Quantity = dishAvailability.Quantity;
            }

            _context.SaveChanges();
        }

        public DishAvailability Delete(int dishAvailId)
        {
            DishAvailability dbDishAvail = _context.DishesAvail.FirstOrDefault(da => da.Id == dishAvailId);

            if (dbDishAvail != null)
            {
                _context.DishesAvail.Remove(dbDishAvail);
                _context.SaveChanges();
            }

            return dbDishAvail;
        }
    }
}