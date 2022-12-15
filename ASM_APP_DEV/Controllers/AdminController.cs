﻿using ASM_APP_DEV.Data;
using ASM_APP_DEV.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Linq;

namespace ASM_APP_DEV.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext context;

        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index() {

          var accounts=  context.Users.ToList();
            return View(accounts);
        }
        [HttpGet]
        public IActionResult Categories()
        {

            var categories = context.Categories.ToList();
            return View(categories);
        }
    }
}
