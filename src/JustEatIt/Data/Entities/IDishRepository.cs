<<<<<<< HEAD
﻿using System.Linq;
using JustEatIt.Models;

namespace JustEatIt.Data.Entities
{
    public interface IDishRepository
    {
        IQueryable<Dish> GetAll { get; }

        int Save(Dish dish);

        Dish Delete(int dishId);
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models
{
    public interface IDishRepository
    {
        IQueryable<Dish> GetAll { get; }
        Dish Save(Dish dish);
        Dish Delete(int dishId);
    }
}
>>>>>>> jovane_r1_i2
