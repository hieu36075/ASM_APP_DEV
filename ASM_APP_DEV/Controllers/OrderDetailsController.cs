using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using ASM_APP_DEV.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ASM_APP_DEV.Controllers
{
    public class OrderDetailsController : Controller
    {
        private ApplicationDbContext context;
        private readonly UserManager<User> userManager;

        public OrderDetailsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var viewModelCart = new ViewModelCart();
            var currentUser = await userManager.GetUserAsync(User);
            viewModelCart.Address = currentUser.Address;
            viewModelCart.FullName = currentUser.FullName;
            viewModelCart.Phone = currentUser.PhoneNumber;
            if (id != 0)
            {
                var Order = context.Orders.SingleOrDefault(o => o.Id == id && o.UserId == currentUser.Id);

                var orderDetails = context.OrderDetails
                             .Where(t => t.Order.UserId == currentUser.Id && t.Order.Id == id).ToList();
                foreach (var item in orderDetails)
                {
                    item.Book = context.Books.SingleOrDefault(b => b.Id == item.IdBook);
                }
                viewModelCart.OrderDetails = orderDetails;
                viewModelCart.Order = Order;

                return View(viewModelCart);
            }
            else
            {
                var orderDetails = context.OrderDetails.Include(t => t.Order)
                    .Where(t => t.Order.OrderStatus == Enums.OrderStatus.Unconfirmed).ToList();

                var Order = context.Orders.SingleOrDefault(o => o.OrderStatus == OrderStatus.Unconfirmed && o.UserId == currentUser.Id);
                foreach (var item in orderDetails)
                {
                    item.Book = context.Books.SingleOrDefault(b => b.Id == item.IdBook);
                }


                viewModelCart.OrderDetails = orderDetails;
                viewModelCart.Order = Order;
            }
            return View(viewModelCart);

        }
        [HttpGet]

        public IActionResult Delete(int id)
        {
            var OrderDetail = context.OrderDetails.Include(t => t.Order)
                .SingleOrDefault(t => t.Id == id && t.Order.OrderStatus == OrderStatus.Unconfirmed);

            context.Remove(OrderDetail);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult BuyBooks(ViewModelCart viewModelCart)
        {
            viewModelCart.Order.PriceOrder = 0;

            foreach (var item in viewModelCart.OrderDetails)
            {
                var bookInDb = context.Books.SingleOrDefault(b => b.Id == item.IdBook);
                var orderDetail = context.OrderDetails.SingleOrDefault(o => o.Id == item.Id);
                if (bookInDb.QuantityBook < orderDetail.Quantity)
                {
                    TempData["err"] = "Quantity Book In Databae: " + bookInDb.QuantityBook;
                    return RedirectToAction("Index", "OrderDetails");
                }
                else
                {
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.Book.PriceBook * item.Quantity;
                    bookInDb.QuantityBook -= orderDetail.Quantity;

                    context.Update(orderDetail);
                    viewModelCart.Order.PriceOrder += orderDetail.Price;
                    context.SaveChanges();
                }

            }
            var orderInDb = context.Orders.SingleOrDefault(o => o.Id == viewModelCart.Order.Id);
            orderInDb.PriceOrder = viewModelCart.Order.PriceOrder;
            orderInDb.OrderStatus = OrderStatus.InProgress;
            context.Update(orderInDb);
            context.SaveChanges();

            return RedirectToAction("Index", "Orders");
        }

        public IActionResult TangSoLuong(int id)
        {
            var orderDetail = context.OrderDetails.Include(o => o.Order).SingleOrDefault(o => o.Id == id);
            var book = context.Books.SingleOrDefault(o => o.Id == orderDetail.IdBook);
            orderDetail.Quantity += 1;
            orderDetail.Price = book.PriceBook * orderDetail.Quantity;
            context.Update(orderDetail);
            orderDetail.Order.PriceOrder = 0;

            foreach (var item in orderDetail.Order.OrderDetails)
            {

                orderDetail.Order.PriceOrder += item.Price;


            }
            context.SaveChanges();

            return RedirectToAction("Index", "OrderDetails");
        }
        public IActionResult TruSoLuong(int id)
        {
            var orderDetail = context.OrderDetails.Include(b => b.Book).Include(o => o.Order).SingleOrDefault(o => o.Id == id);
            var book = context.Books.SingleOrDefault(o => o.Id == orderDetail.IdBook);
            orderDetail.Quantity -= 1;
            orderDetail.Price = book.PriceBook * orderDetail.Quantity;
            context.Update(orderDetail);
            orderDetail.Order.PriceOrder = 0;

            foreach (var item in orderDetail.Order.OrderDetails)
            {

                orderDetail.Order.PriceOrder += item.Price;


            }
            context.SaveChanges();

            return RedirectToAction("Index", "OrderDetails");
        }
    }
}