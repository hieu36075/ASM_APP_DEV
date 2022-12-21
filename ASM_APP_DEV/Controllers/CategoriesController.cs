using ASM_APP_DEV.Data;
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

        public IActionResult Create()
        {
            var categoriesInDb = dbContext.Categories.ToList();

            return View(categoriesInDb);
        }
    }
}
