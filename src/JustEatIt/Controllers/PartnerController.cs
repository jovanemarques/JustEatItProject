using System;
using System.Linq;
using System.Security.Claims;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustEatIt.Controllers
{
    [Authorize(Roles = "Partner,Administrator")]
    public class PartnerController : Controller
    {
        IDishRepository dishRepo;
        IOrderRepository orderRepo;
        IDishAvailabilityRepository dishAvailRepo;

        public PartnerController(IDishRepository dishRepository, IOrderRepository orderRepo, 
            IDishAvailabilityRepository dishAvailRepo)
        {
            this.dishRepo = dishRepository;
            this.orderRepo = orderRepo;
            this.dishAvailRepo = dishAvailRepo;
        }

        public IActionResult Index(DateTime? date)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            DateTime currentDate = (DateTime.Now.Hour <= 2) ? DateTime.Now.AddDays(-1).Date : DateTime.Now.Date;
            DateTime dashboardDate = (date == null) ? currentDate : date.Value.Date;

            var dishAvailList = dishAvailRepo.GetAll.Where(da =>
                da.StartDate.Date == dashboardDate &&
                da.Dish.PartnerId == userId).ToList();

            var orderList = orderRepo.GetAll.Where(o => o.OrderedAt.Date == dashboardDate &&
                o.PartnerId == userId).ToList();

            int dishesSold = 0;
            foreach (var order in orderList)
            {
                dishesSold += order.Items.Sum(i => i.Quantity);
            }

            //var dishAvailList
            return View(new PartnerDashboardViewModel()
            {
                Date = dashboardDate,
                DishesMenu = dishAvailList.Count(),
                OrdersFinished = orderList.Count(o => o.Status != 1),
                PendingOrders = orderList.Count(o => o.Status == 1),
                DishesAvailable = dishAvailList.Sum(da => da.Quantity),
                DishesSold = dishesSold,
                OpenedOrders = orderList.Where(o => o.Status == 1),
                ReturnUrl = $"{Request.Path}{Request.QueryString}",
                IsCurrentDate = (dashboardDate.CompareTo(currentDate) == 0)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeOrderStatus(int id, int status, string returnUrl)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Order order = orderRepo.GetAll.FirstOrDefault(o => o.PartnerId == userId && o.Id == id && o.Status == 1);
            if ((order != null) && (status == 2 || status == 3))
            {
                orderRepo.UpdateStatus(order.Id, status);
            }
            else
            {
                TempData["StatusMessage"] = "#E#:This order do not exist.";
            }

            return Redirect(returnUrl);
        }
    }
}