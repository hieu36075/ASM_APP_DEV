using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APP_DEV.Controllers
{
    public class CategoriesController : Controller
    {
        ApplicationDbContext dbContext;
        UserManager<User> userManager;
        public CategoriesController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var categoriesInDb = dbContext.Categories.Where(c => c.UserId == currentUser.Id).ToList();
            
            return View(categoriesInDb); 
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var currentUser = await userManager.GetUserAsync(User);

            category.UserId = currentUser.Id;
            category.CategoryStatus = CategoryStatus.Unconfirmed;
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }

        public IActionResult Delete(int id)
        {
            var category = dbContext.Categories.FirstOrDefault(c => c.Id == id);
            dbContext.Remove(category);
            dbContext.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
    }
}
