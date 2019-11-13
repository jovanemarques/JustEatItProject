using System.Linq;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Models.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDishAvailabilityRepository _dishAvailabilityRepository;
        private readonly IItemOrderRepository _itemOrderRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(
            IOrderRepository orderRepository,
            UserManager<IdentityUser> userManager,
            IDishAvailabilityRepository dishAvailabilityRepository,
            IItemOrderRepository itemOrderRepository)
        {
            _orderRepository = orderRepository;
            _userManager = userManager;
            _dishAvailabilityRepository = dishAvailabilityRepository;
            _itemOrderRepository = itemOrderRepository;
        }

        [HttpGet]
        [Route("orders")]
        public IActionResult Orders()
        {
            string userId = _userManager.GetUserId(User);

            return View(_orderRepository.GetOrdersForCustomer(userId));
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            var availableDishes = _dishAvailabilityRepository.GetAll();

            var createOrderViewModel = new CreateOrder();

            foreach (var availableDish in availableDishes)
            {
                createOrderViewModel.OrderItems.Add(new OrderItem { DishAvail =  availableDish, Quantity = 0});
            }

            return View(createOrderViewModel);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateOrder order)
        {
            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.Quantity < 0 || orderItem.Quantity > orderItem.DishAvail.Quantity)
                {
                    return View(order);
                }
            }

            var orderItems = order.OrderItems.Where(x => x.Quantity > 0);

            foreach (var orderItem in orderItems)
            {
                _itemOrderRepository.Create(orderItem);
            }

            return RedirectToAction("Orders");
        }
    }
}