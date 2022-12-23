using ASM_APP_DEV.Data;
using ASM_APP_DEV.Models;
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
        [Authorize(Roles = "user")]
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
        public IActionResult Detail(int id)
        {
            var bookInDb = _context.Books.SingleOrDefault(b => b.Id == id);

            return View(bookInDb);
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
