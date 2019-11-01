using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    public class DishController : Controller
    {
        IDishRepository dishRepo;
        public DishController(IDishRepository dishRepository)
        {
            dishRepo = dishRepository;
        }
        // GET: Dish
        public ActionResult Index()
        {
            // check if user is custumer or partner and redirect to right page
            return View("IndexCustomer");
            //return View("IndexPartner");
        }

        public ActionResult IndexPartner()
        {
            // check user and redirect to right page
            return View(dishRepo.GetAll);
        }

        public ActionResult IndexCustomer()
        {
            // check user and redirect to right page
            return View(dishRepo.GetAll);
        }

        // GET: Dish/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dish/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dish/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]Dish dish)
        {
            try
            {
                dishRepo.Save(dish);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dish/Edit/5
        public ActionResult Edit(int dishId)
        {
            dishRepo.GetAll.Where(d => d.Id == dishId);
            return View("Create", dishRepo.GetAll.First());
        }

        // POST: Dish/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int dishId)
        {
            try
            {
                Dish dish = dishRepo.Delete(dishId);
                return RedirectToAction(nameof(Index), dish);
            }
            catch
            {
                return View();
            }
        }
    }
}