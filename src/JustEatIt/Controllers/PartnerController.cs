using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    public class PartnerController : Controller
    {
        IDishRepository dishRepo;

        public PartnerController(IDishRepository dishRepository)
        {
            dishRepo = dishRepository;
        }

        public IActionResult Index()
        {
            User.
            return View();
        }
    }
}