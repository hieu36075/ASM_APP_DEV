using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASM_APP_DEV.Controllers
{
    public class CategoriesController : Controller
    {
        ApplicationDbContext dbContext;
        public CategoriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categoriesInDb = dbContext.Categories.ToList();
            
            return View(categoriesInDb); 
        }

        //Create Category Data
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            category.CategoryStatus = CategoryStatus.Unconfirmed;
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }

        //Delete Category Data
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
            dbContext.Remove(category);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
    }
}
