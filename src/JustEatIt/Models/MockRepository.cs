using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public class MockRepository : IRepository
    {
        private User partner => new User { Id = 1, FirstName = "John", LastName = "Lennon" };
        public IQueryable<Dish> Dishes => new List<Dish> { 
            new Dish {Id = 1, Description = "Pepperoni Pizza", Price = 5.99f, BestBefore = new DateTime(2019, 12, 25), 
                Image = "https://img.buzzfeed.com/thumbnailer-prod-us-east-1/" +
                "dc23cd051d2249a5903d25faf8eeee4c/BFV36537_CC2017_2IngredintDough4Ways-FB.jpg",
                Type = "Pizza", Restaurant = "Italian", Partner = partner, Quantity = 50
            },
            new Dish {Id = 2, Description = "Cheese Pizza", Price = 5.99f, BestBefore = new DateTime(2019, 12, 26),
                Image = "https://www.countrysidecravings.com/wp-content/uploads/2017/03/Three-Cheese-Pizza-2-500x375.jpg",
                Type = "Pizza", Restaurant = "Italian", Partner = partner, Quantity = 25
            }
        }.AsQueryable<Dish>();
    }
}
