using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JustEatIt.Data.Entities;
using JustEatIt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustEatIt.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository _repoUser;

        public AdminUserController(IUserRepository repoUser)
        {
            _repoUser = repoUser;
        }


        public IActionResult Index([Bind("SearchText")] string searchText)
        {
            IEnumerable<UserInfo> users;
            ViewData["currentFilter"] = searchText;

            if ((searchText != null) && (searchText.Trim().Length > 0))
            {
                users = _repoUser.GetAll.Where(d => d.Name.Contains(searchText)
                    || d.Role.Contains(searchText)
                    || d.UserName.Contains(searchText));
            }
            else
            {
                users = _repoUser.GetAll;
            }

            return View(users);
        }

        //public async Task<IActionResult> EditUser(string id, string role, string searchText)
        //{
        //    switch (role)
        //    {
        //        case "Customer":
        //            return await EditCustomer(id, searchText);

        //        case "Partner":
        //            return await EditPartner(id, searchText);

        //        case "Administrator":
        //            return await EditAdmin(id, searchText);
        //    }

        //    return View(nameof(Index), new { searchText = searchText });
        //}

        //public async Task<IActionResult> EditCustomer(string id, string searchText)
        //{
        //    ViewData["currentFilter"] = searchText;

        //    if ((id == null) || (id.Trim() == ""))
        //    {
        //        dish = new Dish();
        //    }
        //    else
        //    {
        //        dish = await _repoDish.GetAll.FirstOrDefaultAsync(t => t.Id == id);
        //        if (dish == null)
        //        {
        //            TempData["StatusMessage"] = "#E#:The selected Dish was not found to edit.";
        //            return RedirectToAction(nameof(IndexDish), new { stringText = searchText });
        //        }
        //    }

        //    ViewBag.TypeList = _repoType.GetAll.OrderBy(t => t.TypeName).Select(t => new { Id = t.Id, Name = t.TypeName }).ToList();
        //    ViewBag.PartnerList = _repoPartner.GetAll.OrderBy(p => p.Name).Select(p => new { Id = p.Id, Name = p.Name }).ToList();

        //    ViewBag.TypeList.Insert(0, new { Id = 0, Name = "Select a Type" });
        //    ViewBag.PartnerList.Insert(0, new { Id = "", Name = "Select a Partner" });


        //}


        //public async Task<IActionResult> EditPartner(string id, string searchText)
        //{


        //}


        //public async Task<IActionResult> EditAdmin(string id, string searchText)
        //{


        //}
    }
}