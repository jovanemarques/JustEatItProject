using System;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Controllers
{
    public class DishController : Controller
    {
        private readonly IDishRepository _dishRepo;
        private readonly IPartnerRepository _partnerRepo;

        public DishController(IDishRepository dishRepository, IPartnerRepository partnerRepository)
        {
            _dishRepo = dishRepository;
            _partnerRepo = partnerRepository;
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
            return View(_dishRepo.GetAll);
        }

        public ActionResult IndexCustomer()
        {
            // check user and redirect to right page
            return View(_dishRepo.GetAll);
        }

        public ActionResult IndexCustomerList()
        {
            // check user and redirect to right page
            return View(_dishRepo.GetAll);
        }

        [HttpPost]
        public String GetDishesByLatLog(String[] ne, String[] sw)
        {
            decimal ne_lat = Decimal.Parse(ne[0]);
            decimal ne_lng = Decimal.Parse(ne[1]);
            decimal sw_lat = Decimal.Parse(sw[0]);
            decimal sw_lng = Decimal.Parse(sw[1]);

            var partners = _partnerRepo.GetAll
            .Where(partner =>
                partner.Latitude >= sw_lat && partner.Longitude >= sw_lng &&
                partner.Latitude <= ne_lat && partner.Longitude <= ne_lng
            ).Include("Dishes").ToList();

            // this could be a ViewModel also
            String json = "";
            json += "[";
            var firstPartner = true;
            foreach (var partner in partners)
            {
                json += firstPartner ? "" : ",";
                json += "   {";
                json += "       \"name\":\"" + partner.Name + "\",";
                json += "       \"location\": { \"lat\": " + partner.Latitude + ", \"lng\": " + partner.Longitude + " },";
                json += "       \"dishes\": [";
                var firstDish = true;
                foreach (var dish in partner.Dishes)
                {
                    json += firstDish ? "" : ",";
                    json += "           \"" + dish.Name + "\"";
                    firstDish = false;
                }
                json += "       ]";
                json += "   }";
                firstPartner = false;
            }
            json += "       ]";

            return json;
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
            var createdDishId = _dishRepo.Save(dish);

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

            _dishRepo.Save(dish);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int dishId)
        {
            IQueryable<Dish> dishes = _dishRepo.GetAll;
            var myDished = dishes.ToList().Where(d => d.Id == dishId);

            return View("Create", dishes.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dishId)
        {
            Dish dish = _dishRepo.Delete(dishId);
            await S3ImageService.RemoveFileFromS3(dishId);

            return RedirectToAction(nameof(Index), dish);
        }
    }
}