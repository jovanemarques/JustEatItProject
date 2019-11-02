using System.Linq;
using System.Security.Claims;
using JustEatIt.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    [Authorize]
    public class PartnerController : Controller
    {
        IDishRepository dishRepo;

        public PartnerController(IDishRepository dishRepository)
        {
            dishRepo = dishRepository;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(dishRepo.GetAll.Where(d => d.Partner.Id == userId));
        }
    }
}