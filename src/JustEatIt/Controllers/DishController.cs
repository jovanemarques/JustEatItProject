using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Controllers
{
    public class DishController : Controller
    {
        private readonly IDishRepository _dishRepo;
        private readonly IDishTypeRepository _typeRepo;
        private readonly IDishAvailabilityRepository _availRepo;
        private readonly IPartnerRepository _partnerRepo;
        private readonly UserManager<CustomUser> _userManager;

        public DishController(IDishRepository dishRepository, IDishTypeRepository typeRepository, 
            IDishAvailabilityRepository availabilityRepository, UserManager<CustomUser> userManager, IPartnerRepository partnerRepository)
        {
            _dishRepo = dishRepository;
            _typeRepo = typeRepository;
            _availRepo = availabilityRepository;
            _userManager = userManager;
            _partnerRepo = partnerRepository;
        }

        public ActionResult Index()
        {
            // check if user is custumer or partner and redirect to right page
            return View("IndexCustomer");
            //return View("IndexPartner");
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> IndexPartner()
        {
            // check user and redirect to right page
            var user = await _userManager.GetUserAsync(User);
            return View(_dishRepo.GetAll.Where(d => d.Partner.Id.Equals(user.Id)));
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
            );

            var dishes = _dishRepo.GetAll
            .Where(d =>
                d.Partner.Latitude >= sw_lat && d.Partner.Longitude >= sw_lng &&
                d.Partner.Latitude <= ne_lat && d.Partner.Longitude <= ne_lng
            )
            .OrderBy(d => d.PartnerId);

            String json = "";
            json += "[";
            String previousPartnerId = "";
            var firstPartner = true;
            var firstDish = true;
            foreach (var dish in dishes)
            {
                if (previousPartnerId != dish.Partner.Id)
                {
                    if (!firstPartner)
                    {
                        json += "       ]";
                        json += "   }";
                        json += ",";
                    }
                    json += "   {";
                    json += "       \"id\":\"" + dish.Partner.Id + "\",";
                    json += "       \"name\":\"" + dish.Partner.Name + "\",";
                    json += "       \"location\": { \"lat\": " + dish.Partner.Latitude + ", \"lng\": " + dish.Partner.Longitude + " },";
                    json += "       \"dishes\": [";
                    firstDish = true;
                    firstPartner = false;
                }
                foreach (var dishAv in dish.DishAvailabilities.Where(da => da.StartDate.Date >= DateTime.Now.Date))
                {
                    json += firstDish ? "" : ",";
                    json += "           \"" + dishAv.Dish.Name + "\"";
                    firstDish = false;
                }
                
                previousPartnerId = dish.Partner.Id;
            }
            json += "       ]";
            json += "   }";
            json += "]";

            return json;
        }

        [Authorize(Roles = "Partner")]
        public ActionResult Create()
        {
            ViewBag.TypeList = _typeRepo.GetAll.OrderBy(t => t.TypeName).Select(t => new { Id = t.Id, Name = t.TypeName }).ToList();
            ViewBag.TypeList.Insert(0, new { Id = 0, Name = "Select a Type" });

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Partner")]
        public async Task<ActionResult> Create([FromForm]Dish dish)
        {
            var user = await _userManager.GetUserAsync(User);
            dish.PartnerId = user.Id;

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
            return RedirectToAction(nameof(IndexPartner));
        }

        [Authorize(Roles = "Partner")]
        public ActionResult Edit(int dishId)
        {
            var myDished = _dishRepo.GetAll.FirstOrDefault(d => d.Id == dishId);

            if (myDished == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to edit.";
                return RedirectToAction(nameof(IndexPartner));
            }

            ViewBag.TypeList = _typeRepo.GetAll.OrderBy(t => t.TypeName).Select(t => new { Id = t.Id, Name = t.TypeName }).ToList();
            ViewBag.TypeList.Insert(0, new { Id = 0, Name = "Select a Type" });

            return View("Create", myDished);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Partner")]
        public async Task<ActionResult> Delete(int id)
        {
            Dish dish = _dishRepo.Delete(id);
            await S3ImageService.RemoveFileFromS3(id);

            return RedirectToAction(nameof(IndexPartner), dish);
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> IndexDishAvail(int dishId)
        {
            var dish = _dishRepo.GetAll.FirstOrDefault(d => d.Id == dishId);
            if (dish == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to see their Availability.";
                return RedirectToAction(nameof(IndexPartner));
            }

            var user = await _userManager.GetUserAsync(User);
            if (dish.Partner.Id != user.Id)
            {
                TempData["StatusMessage"] = "#E#:You are not authorized to access this dish.";
                return RedirectToAction(nameof(IndexPartner));
            }
            
            ViewData["availDish"] = dish;
            return View(_availRepo.GetAll.Where(a => a.DishId == dishId).OrderByDescending(a => a.StartDate));
        }

        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> CreateEditDishAvail(int? id, int dishId)
        {
            DishAvailability avail;

            var dish = _dishRepo.GetAll.FirstOrDefault(d => d.Id == dishId);
            if (dish == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to see their Availability.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (dish.Partner.Id != user?.Id)
            {
                TempData["StatusMessage"] = "#E#:You are not authorized to access this dish.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            if ((id == null) || (id <= 0))
            {
                avail = new DishAvailability();
                avail.StartDate = DateTime.Now.Date;
                avail.EndDate = DateTime.Now.Date;
                avail.Dish = dish;
                avail.DishId = dish.Id;
            }
            else
            {
                avail = await _availRepo.GetAll.FirstOrDefaultAsync(t => t.Id == id);
                if (avail == null)
                {
                    TempData["StatusMessage"] = "#E#:The selected Dish Availability was not found to edit.";
                    return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
                }

                if (avail.Dish?.Id != dish.Id)
                {
                    TempData["StatusMessage"] = "#E#:You are not authorized to access this dish availability.";
                    return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
                }
            }

            ViewData["availDish"] = avail.Dish;
            return View(avail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> CreateEditDishAvail(int? id, int dishId, DishAvailability avail)
        {
            var dish = await _dishRepo.GetAll.FirstOrDefaultAsync(d => d.Id == dishId);
            if (dish == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to edit/create their Availability.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (dish.Partner.Id != user?.Id)
            {
                TempData["StatusMessage"] = "#E#:You are not authorized to access this dish.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            if ((avail.Id != id) || (avail.DishId != dishId))
            {
                TempData["StatusMessage"] = "#E#:Invalid information to save the Dish Availability.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            // If it is a edit, verify the quantity. Otherwise, set the quantity as the total
            if (id != 0)
            {
                var dbAvail = await _availRepo.GetAll.FirstOrDefaultAsync(d => d.Id == id);
                if (dbAvail == null)
                {
                    TempData["StatusMessage"] = "#E#:Unable to find the Dish Availability to edit.";
                    return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
                }

                var dishSold = dbAvail.QuantityTotal - dbAvail.Quantity;
                if (dishSold > avail.QuantityTotal)
                {
                    ModelState.AddModelError("QuantityTotal", $"You cannot define the quantity as less then ${dishSold}, as they was already sold.");
                }
                else
                {
                    avail.Quantity = avail.QuantityTotal - dishSold;
                }

                if ((dishSold > 0) && ((dbAvail.StartDate.CompareTo(avail.StartDate) != 0) || (dbAvail.StartDate.CompareTo(avail.StartDate) != 0)))
                {
                    ModelState.AddModelError("StartDate", "You cannot change the Available from or Expire date when this dish was was already sold.");
                }
            }
            else
            {
                avail.Quantity = avail.QuantityTotal;
            }

            // Validate all the fields for both new and edit
            var interval = avail.EndDate.Subtract(avail.StartDate).Hours;
            if (avail.StartDate.CompareTo(avail.EndDate) >= 0)
            {
                ModelState.AddModelError("StartDate", $"The Available from must be less than Expire date field.");
            }
            else if (interval < 2)
            {
                ModelState.AddModelError("StartDate", $"The Dish Availability should have at least two hours of interval.");
            }
            else if (interval >= 20)
            {
                ModelState.AddModelError("StartDate", $"The Dish Availability cannot exceed twenty hours of interval.");
            }

            if (avail.StartDate.CompareTo(DateTime.Now) < 0)
            {
                ModelState.AddModelError("StartDate", $"The available from field must be greater than the current date.");
            }
            
            if ((avail.StartDate.Hour < 6) || (avail.StartDate.Hour >= 22))
            {
                ModelState.AddModelError("StartDate", $"The Start Date must be between 6:00AM and 21:59PM.");
            }

            if ((avail.EndDate.Hour > 2) && (avail.EndDate.Hour <= 7)) 
            {
                ModelState.AddModelError("EndDate", $"The End Date must be between 8:00AM and 2:59AM.");
            }
            

            // Validate if we have at least 

            if (ModelState.IsValid)
            {
                _availRepo.Save(avail);
                TempData["StatusMessage"] = $"The Dish Availability information was saved.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            ViewData["availDish"] = dish;
            return View(avail);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Partner")]
        public async Task<IActionResult> DeleteDishAvail(int id, int dishId)
        {
            var avail = await _availRepo.GetAll.FirstOrDefaultAsync(d => d.Id == id);
            if (avail == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish Availability.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            if (avail.Quantity < avail.QuantityTotal)
            {
                TempData["StatusMessage"] = "#E#:You cannot delete this Dish Availability, as it was already sold. Contact your support in case of problems.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (avail.Dish.PartnerId != user?.Id) 
            {
                TempData["StatusMessage"] = "#E#:You are not authorized to access this dish.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            avail = _availRepo.Delete(id);
            if (avail != null)
            {
                TempData["StatusMessage"] = $"The Dish Availalibity was deleted.";
            }
            else
            {
                TempData["StatusMessage"] = "#E#:Unable to find this item to delete.";
            }

            return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
        }
    }
}