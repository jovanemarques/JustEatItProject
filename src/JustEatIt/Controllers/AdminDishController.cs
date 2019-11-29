using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JustEatIt.Data;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using Microsoft.AspNetCore.Authorization;

namespace JustEatIt.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminDishController : Controller
    {
        private readonly IDishTypeRepository _repoType;
        private readonly IDishRepository _repoDish;
        private readonly IDishAvailabilityRepository _repoAvail;
        private readonly IPartnerRepository _repoPartner;

        public AdminDishController(IDishTypeRepository repoType, IDishRepository repoDish, IDishAvailabilityRepository repoAvail,
            IPartnerRepository repoPartner)
        {
            _repoType = repoType;
            _repoDish = repoDish;
            _repoAvail = repoAvail;
            _repoPartner = repoPartner;
        }

        public IActionResult Index()
        {
            return View(_repoType.GetAll);
        }
        
        public async Task<IActionResult> CreateEditDishType(int? id)
        {
            DishType dishType;

            if ((id == null) || (id <= 0))
            {
                dishType = new DishType();
            }
            else
            {
                dishType = await _repoType.GetAll.FirstOrDefaultAsync(t => t.Id == id);
                if (dishType == null)
                {
                    TempData["StatusMessage"] = "#E#:The selected Dish Type was not found to edit.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(dishType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditDishType(int? id, [Bind("Id,TypeName")] DishType dishType)
        {
            if (id != dishType.Id)
            {
                TempData["StatusMessage"] = "#E#:Invalid information to save the Dish Type.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                _repoType.Save(dishType);
                TempData["StatusMessage"] = $"The Dish Type information was saved.";
                return RedirectToAction(nameof(Index));
            }
            return View(dishType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDishType(int id)
        {
            var dishType = _repoType.Delete(id);
            if (dishType != null)
            {
                TempData["StatusMessage"] = $"The Dish Type '{dishType.TypeName}' was deleted.";
            }
            else
            {
                TempData["StatusMessage"] = "#E#:Unable to find this item to delete.";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult IndexDish([Bind("SearchText")] string searchText)
        {
            IEnumerable<Dish> dishes;
            ViewData["currentFilter"] = searchText;

            if ((searchText != null) && (searchText.Trim().Length > 0))
            {
                dishes = _repoDish.GetAll.Where(d => d.Name.Contains(searchText)
                    || d.Type.TypeName.Contains(searchText)
                    || d.Partner.Name.Contains(searchText));
            }
            else
            {
                dishes = _repoDish.GetAll;
            }

            return View(dishes);
        }

        public async Task<IActionResult> CreateEditDish(int? id, string searchText)
        {
            Dish dish;
            ViewData["currentFilter"] = searchText;

            if ((id == null) || (id <= 0))
            {
                dish = new Dish();
            }
            else
            {
                dish = await _repoDish.GetAll.FirstOrDefaultAsync(t => t.Id == id);
                if (dish == null)
                {
                    TempData["StatusMessage"] = "#E#:The selected Dish was not found to edit.";
                    return RedirectToAction(nameof(IndexDish), new { stringText = searchText });
                }
            }

            ViewBag.TypeList = _repoType.GetAll.OrderBy(t => t.TypeName).Select(t => new { Id = t.Id, Name = t.TypeName }).ToList();
            ViewBag.PartnerList = _repoPartner.GetAll.OrderBy(p => p.Name).Select(p => new { Id = p.Id, Name = p.Name }).ToList();

            ViewBag.TypeList.Insert(0, new { Id = 0, Name = "Select a Type" });
            ViewBag.PartnerList.Insert(0, new { Id = "", Name = "Select a Partner" });

            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditDish(int? id, Dish dish, string searchText)
        {
            ViewData["currentFilter"] = searchText;

            if (id != dish.Id)
            {
                TempData["StatusMessage"] = "#E#:Invalid information to save the Dish.";
                return RedirectToAction(nameof(IndexDish), new { stringText = searchText });
            }

            if (ModelState.IsValid)
            {
                _repoDish.Save(dish);
                TempData["StatusMessage"] = $"The Dish information was saved.";
                return RedirectToAction(nameof(IndexDish), new { stringText = searchText });
            }

            ViewBag.TypeList = _repoType.GetAll.OrderBy(t => t.TypeName).Select(t => new { Id = t.Id, Name = t.TypeName }).ToList();
            ViewBag.PartnerList = _repoPartner.GetAll.OrderBy(p => p.Name).Select(p => new { Id = p.Id, Name = p.Name }).ToList();

            ViewBag.TypeList.Insert(0, new { Id = 0, Name = "Select a Type" });
            ViewBag.PartnerList.Insert(0, new { Id = "", Name = "Select a Partner" });

            return View(dish);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDish(int id, string searchText)
        {
            var dish = _repoDish.Delete(id);
            if (dish != null)
            {
                TempData["StatusMessage"] = $"The Dish '{dish.Name}' was deleted.";
            }
            else
            {
                TempData["StatusMessage"] = "#E#:Unable to find this item to delete.";
            }

            return RedirectToAction(nameof(IndexDish), new { stringText = searchText });
        }

        public IActionResult IndexDishAvail(int dishId, [Bind("SearchText")] string searchText)
        {
            ViewData["currentFilter"] = searchText;

            if ((ViewData["availDish"] = _repoDish.GetAll.FirstOrDefault(d => d.Id == dishId)) == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to see their Availability.";
                return RedirectToAction(nameof(IndexDish));
            }

            return View(_repoAvail.GetAll.Where(a => a.DishId == dishId));
        }

        public async Task<IActionResult> CreateEditDishAvail(int? id, int dishId)
        {
            DishAvailability avail;

            if ((id == null) || (id <= 0))
            {
                avail = new DishAvailability();
                avail.Dish = _repoDish.GetAll.FirstOrDefault(d => d.Id == dishId);
                avail.DishId = avail.Dish.Id;
            }
            else
            {
                avail = await _repoAvail.GetAll.FirstOrDefaultAsync(t => t.Id == id);
                if (avail == null)
                {
                    TempData["StatusMessage"] = "#E#:The selected Dish Availability was not found to edit.";
                    return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
                }
            }

            if (avail?.Dish == null)
            {
                TempData["StatusMessage"] = "#E#:An error occurred retrieving the Dish, please try again.";
                return RedirectToAction(nameof(IndexDish));
            }

            ViewData["availDish"] = avail.Dish;

            return View(avail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEditDishAvail(int? id, int dishId, DishAvailability avail)
        {
            if (id != avail.Id)
            {
                TempData["StatusMessage"] = "#E#:Invalid information to save the Dish Availability.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            if (ModelState.IsValid)
            {
                _repoAvail.Save(avail);
                TempData["StatusMessage"] = $"The Dish Availability information was saved.";
                return RedirectToAction(nameof(IndexDishAvail), new { dishId = dishId });
            }

            if ((ViewData["availDish"] = _repoDish.GetAll.FirstOrDefault(d => d.Id == dishId)) == null)
            {
                TempData["StatusMessage"] = "#E#:Unable to find the Dish to create/edit their Availability.";
                return RedirectToAction(nameof(IndexDish));
            }

            return View(avail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteDishAvail(int id, int dishId)
        {
            var dish = _repoDish.Delete(id);
            if (dish != null)
            {
                TempData["StatusMessage"] = $"The Dish Availalibity was deleted.";
            }
            else
            {
                TempData["StatusMessage"] = "#E#:Unable to find this item to delete.";
            }

            return RedirectToAction(nameof(IndexDish), new { dishId = dishId });
        }
    }
}
