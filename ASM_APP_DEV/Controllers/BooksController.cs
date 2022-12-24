using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using ASM_APP_DEV.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace ASM_APP_DEV.Controllers
{

    public class BooksController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public BooksController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string SearchString)
        {
            if (!String.IsNullOrEmpty(SearchString))
            {

                var books = _context.Books.Where(s => s.NameBook.Contains(SearchString));
                return View(await books.ToListAsync());

            }
            else
            {
                var books = _context.Books.ToList();
                return View(books);
            }
           
        }
        [HttpGet]

        public async Task<IActionResult> IndexAdmin()
        {
            var currentUser = await userManager.GetUserAsync(User);

            //var booksInDb = _dbContext.Books.ToList();
            IEnumerable<Book> booksInDb = _context.Books.Where(b => b.UserId == currentUser.Id).ToList();

            return View(booksInDb);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var categoriesInDb = _context.Categories
                .Where(c => c.CategoryStatus == CategoryStatus.Successful && c.UserId == currentUser.Id).ToList();
            CategoriesBookViewModel categoryBook = new CategoriesBookViewModel();
            categoryBook.Categories = categoriesInDb;
            return View(categoryBook);

        }

            [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            var currentUser = await userManager.GetUserAsync(User);

            book.UserId = currentUser.Id;
            _context.Add(book);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
        }
        [HttpPost]
        public IActionResult Edit(Book book)
        {

            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == book.Id);
            bookInDb.NameBook = book.NameBook;
            bookInDb.PriceBook = book.PriceBook;
            bookInDb.Image = book.Image;
            bookInDb.InformationBook = book.InformationBook;
            bookInDb.QuantityBook = book.QuantityBook;


            _context.Update(bookInDb);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var bookInDb = _context.Books.FirstOrDefault(c => c.Id == id);
            var orderDetailInDB = _context.OrderDetails.SingleOrDefault(b => b.IdBook== id);
            _context.Remove(orderDetailInDB);
            _context.Remove(bookInDb);
            _context.SaveChanges();
            return RedirectToAction("IndexAdmin", "Books");
        }

        [HttpGet]
        public IActionResult Help()
        {
            return View();
        }
    }
}
