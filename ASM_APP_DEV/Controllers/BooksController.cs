using ASM_APP_DEV.Data;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Authorization;
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
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {

            var newBook = new Book();
            newBook.NameBook = book.NameBook;
            newBook.InformationBook = book.InformationBook;
            newBook.QuantityBook = book.QuantityBook;
            newBook.PriceBook = book.PriceBook;


            _context.Add(newBook);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit() {
            return View();
}
        [HttpPost]
        public IActionResult Edit(Book book)
        {

            var newBook = new Book();
            newBook.NameBook = book.NameBook;
            newBook.InformationBook = book.InformationBook;
            newBook.QuantityBook = book.QuantityBook;
            newBook.PriceBook = book.PriceBook;


            _context.Add(newBook);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}
