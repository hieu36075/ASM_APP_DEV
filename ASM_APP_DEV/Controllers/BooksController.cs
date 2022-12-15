using ASM_APP_DEV.Data;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ASM_APP_DEV.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //var booksInDb = _dbContext.Books.ToList();
            IEnumerable<Book> booksInDb = _context.Books.ToList();

            return View(booksInDb);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            //var booksInDb = _dbContext.Books.ToList();
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
        }

    }
}
