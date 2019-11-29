using System.Linq;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using JustEatIt.Models.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JustEatIt.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDishRepository _dishRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IDishAvailabilityRepository _dishAvailabilityRepository;
        private readonly UserManager<CustomUser> _userManager;

        public OrderController(
            IOrderRepository orderRepository,
            UserManager<CustomUser> userManager,
            IDishRepository dishRepository,
            IDishAvailabilityRepository dishAvailabilityRepository,
            IPartnerRepository partnerRepository)
        {
            _orderRepository = orderRepository;
            _dishRepository = dishRepository;
            _userManager = userManager;
            _dishAvailabilityRepository = dishAvailabilityRepository;
            _partnerRepository = partnerRepository;
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
            var availableDishes = _dishAvailabilityRepository.GetAll;

            var createOrderViewModel = new CreateOrder();

            foreach (var availableDish in availableDishes)
            {
                createOrderViewModel.OrderItems.Add(new OrderItem { DishAvail = availableDish, Quantity = 0 });
            }

            return View(createOrderViewModel);
        }
/*
        [HttpGet]
        [Route("createByPartner/{partnerId}")]
        public IActionResult CreateByPartner(string partnerId)
        {
            var partners = _partnerRepository.GetAll;
            var partner = partners.Where(p => p.Id == partnerId).FirstOrDefault();
            var availableDishes = partner.Dishes;

            var createOrderViewModel = new CreateOrder();

            foreach (var availableDish in availableDishes)
            {
                createOrderViewModel.OrderItems.Add(new OrderItem { DishAvail = availableDish, Quantity = 0 });
            }

            return View(createOrderViewModel);
        }*/

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CreateOrder createOrder)
        {
            foreach (var orderItem in createOrder.OrderItems)
            {
                if (orderItem.Quantity < 0 || orderItem.Quantity > orderItem.DishAvail.Quantity)
                {
                    return View(createOrder);
                }
            }

            var orderItems = createOrder.OrderItems.Where(x => x.Quantity > 0).ToList();
            string userId = _userManager.GetUserId(User);

            if (orderItems.Count() <= 0)
            {
                ModelState.AddModelError("", "Select at least one dish.");
                return RedirectToAction("Create");
            }

            var order = new Order
            {
                CustomerId = userId,
                Items = orderItems,
                Status = 1,
                PartnerId = _dishRepository.GetAll.FirstOrDefault(d => d.Id ==
                    orderItems.FirstOrDefault().DishAvail.DishId)?.PartnerId
            };

            TempData["order"] = JsonConvert.SerializeObject(order);

            return View("CreditCard", new CreditCard());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult OrderDetails(int id)
        {
            var order = _orderRepository.GetOrderById(id);

            return View(order);
        }

        [HttpPost]
        [Route("creditcard")]
        public IActionResult ConfirmPayment(CreditCard creditCard)
        {
            if (ModelState.IsValid)
            {
                if (TempData["order"] is string s)
                {
                    var order = JsonConvert.DeserializeObject<Order>(s);
                    _orderRepository.Create(order);
                }

                return RedirectToAction("Orders");
            }

            return View("CreditCard", creditCard);
        }
    }
}