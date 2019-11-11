using System;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Services;
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

        [HttpPost]
        public String GetDishesByLatLog(String lat, String lng)
        {
            //return dishes based in the location given
            return @"[
                    {
                        name:'Pizza Pepperoni',
                        partner_name:'Pizza Pizza',
                        partner_location:{ lat: 43.6532, lng: -79.3832 }
                    }
                ]";
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

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

            dishRepo.Save(dish);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int dishId)
        {
            IQueryable<Dish> dishes = dishRepo.GetAll;
            var myDished = dishes.ToList().Where(d => d.Id == dishId);

            return View("Create", dishes.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dishId)
        {
            Dish dish = dishRepo.Delete(dishId);
            await S3ImageService.RemoveFileFromS3(dishId);

            return RedirectToAction(nameof(Index), dish);
        }
    }
}