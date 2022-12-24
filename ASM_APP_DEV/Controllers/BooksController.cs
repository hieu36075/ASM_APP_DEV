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
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult IndexAdmin()
        {
            //var booksInDb = _dbContext.Books.ToList();
            IEnumerable<Book> booksInDb = _context.Books.ToList();

            return View(booksInDb);
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categoriesInDb = _context.Categories.Where(c => c.CategoryStatus == CategoryStatus.Successful).ToList();
            CategoriesBookViewModel categoryBook = new CategoriesBookViewModel();
            categoryBook.Categories = categoriesInDb;
            return View(categoryBook);

        }

            [HttpPost]
        public IActionResult Create(Book book)
        {
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

    }
}
