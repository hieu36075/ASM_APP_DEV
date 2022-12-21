using ASM_APP_DEV.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASM_APP_DEV.Controllers
{
    public class BookController : Controller
    {
        ApplicationDbContext dbContext;

        public BookController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var booksInDb = dbContext.Books.ToList();
            return View(booksInDb);
        }
    }
}
