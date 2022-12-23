using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ASM_APP_DEV.Controllers
{

   
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext context;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;

		public AdminController(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
			_userManager = userManager;
			_roleManager = roleManager;
            this.context = context;
        }
        [HttpGet]
		//Get account neu co id thi se tim theo role
        public IActionResult Index(int id) {

			if (id == 1)
			{
				var accountsInDb = _userManager.GetUsersInRoleAsync("user").Result;
				return View( accountsInDb);
			}
			else if(id == 2)
			{
				var accountsInDb = _userManager.GetUsersInRoleAsync("storeOwner").Result;
				return View(accountsInDb);
			}
			else {
				var accounts = context.Users.ToList();
				return View(accounts);
			}
        }
        [HttpGet]
			//get categories 
        public IActionResult Categories()
        {
            var categories = context.Categories
                .Where(categories => categories.CategoryStatus == CategoryStatus.Unconfirmed).ToList();

            return View(categories);
        }

        public IActionResult AcceptCategory(int id)
        {

            var categoryInDb = context.Categories.SingleOrDefault(o => o.Id == id);

            categoryInDb.CategoryStatus = CategoryStatus.Successful;
            context.SaveChanges();
            return RedirectToAction("Categories");
        }
        public IActionResult RejectCategory(int id)
        {

            var categoryInDb = context.Categories.SingleOrDefault(o => o.Id == id);

            context.Remove(categoryInDb);
            context.SaveChanges();

            return RedirectToAction("Categories");
        }
    }
}
