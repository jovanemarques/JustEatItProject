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
        IRepository repository;
        public DishController(IRepository repo)
        {
            repository = repo;
        }
        // GET: Dish
        public ActionResult Index()
        {
            return View(repository.Dishes);
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dish/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dish/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dish/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dish/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}