using System;
using System.Linq;
using System.Security.Claims;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var curDate = DateTime.Now;

            var dishAvailList = dishAvailRepo.GetAll.Where(da =>
                da.StartDate <= curDate && da.EndDate >= curDate &&
                da.Dish.PartnerId == userId).ToList();

            var orderList = orderRepo.GetAll.Where(o =>
                o.PartnerId == userId);

            int dishesSold = 0;
            foreach (var order in orderList)
            {
                dishesSold += order.Items.Sum(i => i.Quantity);
            }

            //var dishAvailList
            return View(new PartnerDashboardViewModel()
            {
                Date = curDate,
                DishesMenu = dishAvailList.Count(),
                OrdersFinished = orderList.Count(o => o.Status != 1),
                PendingOrders = orderList.Count(o => o.Status == 1),
                DishesAvailable = dishAvailList.Sum(da => da.Quantity),
                DishesSold = dishesSold,
                OpenedOrders = orderList.Where(o => o.Status == 1)
            });
        }

        public IActionResult ChangeOrderStatus(int id, int status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Order order = orderRepo.GetAll.FirstOrDefault(o => o.PartnerId == userId && o.Id == id);
            if ((order != null) && (status == 2 || status == 3))
            {
                orderRepo.UpdateStatus(order.Id, status);
            }

            return RedirectToAction("Index");
        }
    }
}