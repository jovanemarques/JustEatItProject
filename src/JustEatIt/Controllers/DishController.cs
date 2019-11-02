<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Services;
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
        public async Task<ActionResult> Create([FromForm]Dish dish)
        {
            var createdDishId = dishRepo.Save(dish);

            if (dish.File != null)
            {
                var fileName = System.IO.Path.GetFileName(dish.File.FileName);
                string fileLocation;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                await using (var localFile = System.IO.File.OpenWrite(fileName))
                {
                    fileLocation = localFile.Name;
                    await using var uploadedFile = dish.File.OpenReadStream();
                    uploadedFile.CopyTo(localFile);
                }

                await S3ImageService.UploadFileToS3(createdDishId, fileLocation);

                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Dish/Edit/5
        public ActionResult Edit(int dishId)
        {
            IQueryable<Dish> dishes = dishRepo.GetAll;
            var myDished = dishes.ToList().Where(d => d.Id == dishId);
            
            return View("Create", dishes.First());
        }

        // POST: Dish/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dishId)
        {
            Dish dish = dishRepo.Delete(dishId);
            await S3ImageService.RemoveFileFromS3(dishId);

            return RedirectToAction(nameof(Index), dish);
        }
    }
=======
﻿using System;
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
            dishRepo.Save(dish);
            return RedirectToAction(nameof(Index));
        }

        // GET: Dish/Edit/5
        public ActionResult Edit(int dishId)
        {
            IQueryable<Dish> dishes = dishRepo.GetAll.Where(d => d.Id == dishId);
            return View("Create", dishes.First());
        }

        // POST: Dish/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int dishId)
        {
            Dish dish = dishRepo.Delete(dishId);
            return RedirectToAction(nameof(Index), dish);
        }
    }
>>>>>>> jovane_r1_i2
}