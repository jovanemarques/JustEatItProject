using JustEatIt.Models;
<<<<<<< HEAD
using System.Linq;
=======
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> jovane_r1_i2

namespace JustEatIt.Data.Entities
{
    public class EFDishRepository : IDishRepository
    {
        private AppDataDbContext context;

        public EFDishRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Dish> GetAll => context.Dish;

<<<<<<< HEAD
        public int Save(Dish dish)
        {
            if (dish.Id == 0)
            {
                var test = context.Dish.Add(dish);
                context.SaveChanges();

                return test.Entity.Id;
            }

            Dish dbDish = context.Dish.FirstOrDefault(r => r.Id == dish.Id);
            if (dbDish != null)
            {
                dbDish.Name = dish.Name;
                dbDish.Description = dish.Description;
                dbDish.Price = dish.Price;
                dbDish.Quantity = dish.Quantity;
                dbDish.BestBefore = dish.BestBefore;
                dbDish.Image = dish.Image;
                dbDish.Type = dish.Type;
                dbDish.Restaurant = dish.Restaurant;
            }

            context.SaveChanges();

            return dbDish.Id;
=======
        public Dish Save(Dish dish)
        {
            if (dish.Id == 0)
            {
                context.Dish.Add(dish);
            }
            else
            {
                Dish dbDish = context.Dish.FirstOrDefault(r => r.Id == dish.Id);
                if (dbDish != null)
                {
                    dbDish.Name = dish.Name;
                    dbDish.Description = dish.Description;
                    dbDish.Price = dish.Price;
                    dbDish.Quantity = dish.Quantity;
                    dbDish.BestBefore = dish.BestBefore;
                    dbDish.Image = dish.Image;
                    dbDish.Type = dish.Type;
                    dbDish.Restaurant = dish.Restaurant;
                }
            }
            context.SaveChanges();
            return dish;
>>>>>>> jovane_r1_i2
        }

        public Dish Delete(int id)
        {
            Dish dbDish = context.Dish.FirstOrDefault(r => r.Id == id);
            if (dbDish != null)
            {
                context.Dish.Remove(dbDish);
                context.SaveChanges();
            }
            return dbDish;
        }

    }
}
