﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JustEatIt.Models;
using Microsoft.AspNetCore.Authorization;

namespace JustEatIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult IndexRole()
        {
            if (User.IsInRole("Customer"))
            {
                return View(nameof(Index));
            }
            else if (User.IsInRole("Partner"))
            {
                return RedirectToAction("Index", "Partner");
            }
            else if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
