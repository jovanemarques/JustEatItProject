using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFDishRepository : IDishRepository
    {
        private AppDataDbContext context;

        public EFDishRepository(AppDataDbContext context)
        {
            this.context = context;
        }

        //public IQueryable<Dish> GetAll => context.Dish;
        public IQueryable<Dish> GetAll => new List<Dish> { 
                    new Dish {Id = 1, Name = "Pepperoni Pizza", Description = "Delicious pizza", Price = 5.99f, 
                        BestBefore = new DateTime(2019, 12, 25), 
                        Image = "https://img.buzzfeed.com/thumbnailer-prod-us-east-1/" +
                        "dc23cd051d2249a5903d25faf8eeee4c/BFV36537_CC2017_2IngredintDough4Ways-FB.jpg",
                        Type = "Pizza", Restaurant = "Italian", Quantity = 50
                    },
                    new Dish {Id = 2, Name = "Cheese Pizza", Description = "Delicious Pizza", Price = 5.99f, BestBefore = new DateTime(2019, 12, 26),
                        Image = "https://www.countrysidecravings.com/wp-content/uploads/2017/03/Three-Cheese-Pizza-2-500x375.jpg",
                        Type = "Pizza", Restaurant = "Italian", Quantity = 25
                    }
                }.AsQueryable<Dish>();

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
                    dbDish.Description = dish.Description;
                    dbDish.Partner = dish.Partner;
                }
            }
            context.SaveChanges();
            return dish;
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
