using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using ASM_APP_DEV.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_APP_DEV.Controllers
{
    public class OrdersController : Controller
    {
		private ApplicationDbContext context;
        private readonly UserManager<User> userManager;


        public OrdersController(ApplicationDbContext context,  UserManager<User> userManager)
		{
            this.userManager = userManager;

            this.context = context;
		}
		public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var oderInDb = context.Orders.Where(o => o.UserId == currentUser.Id ).ToList();
            return View(oderInDb);

        }
        [HttpGet]
		[Authorize(Roles = "user")]

		public async Task<IActionResult> Create(int id) {

            var currentUser = await userManager.GetUserAsync(User);
            var orderUnconfirmInDb = context.Orders.Include(o => o.OrderDetails)
                .SingleOrDefault(o => o.OrderStatus == OrderStatus.Unconfirmed && o.UserId == currentUser.Id);

            Book bookInDB = context.Books.SingleOrDefault(t => t.Id == id);
                if (bookInDB.QuantityBook - 1 < 0)
                {
                    return BadRequest();
                }
			if (orderUnconfirmInDb == null)
            {
                
                var order = new Order();
                order.UserId = currentUser.Id;
                order.OrderStatus = Enums.OrderStatus.Unconfirmed;
                order.PriceOrder = bookInDB.PriceBook;
                order.UserId = currentUser.Id;
                context.Add(order);
                context.SaveChanges();

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.IdBook = bookInDB.Id;
                orderDetail.IdOrder = order.Id;
                orderDetail.Quantity = 1;
                orderDetail.Price = bookInDB.PriceBook;
                orderDetail.Order = order;
                orderDetail.Book = bookInDB;

                context.Add(orderDetail);
                context.SaveChanges();
                order.OrderDetails.Add(orderDetail);
				context.SaveChanges();

			}
			else
            {

                var orderDetailInDb = orderUnconfirmInDb.OrderDetails.SingleOrDefault(o => o.IdBook == id);
                if (orderDetailInDb == null) {
                
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.IdBook = bookInDB.Id;
                orderDetail.IdOrder = orderUnconfirmInDb.Id;
                orderDetail.Quantity = 1;

                    orderDetail.Price = bookInDB.PriceBook;
                orderUnconfirmInDb.OrderDetails.Add(orderDetail);
                    context.Add(orderDetail);
                    context.SaveChanges();
                    orderUnconfirmInDb.PriceOrder = 0;
                    foreach (var item in orderUnconfirmInDb.OrderDetails)
                    {

                        orderUnconfirmInDb.PriceOrder += item.Price;
                    }
                }
                else
                {
                    orderDetailInDb.Quantity += 1;

                    orderDetailInDb.Price = bookInDB.PriceBook * orderDetailInDb.Quantity;
                    orderUnconfirmInDb.PriceOrder = 0;
                    foreach (var orderDetail in orderUnconfirmInDb.OrderDetails)
                    {
                       
                        orderUnconfirmInDb.PriceOrder += orderDetail.Price;
                    }
                }
				context.SaveChanges();

			}
			context.SaveChanges();

            return RedirectToAction("Index", "OrderDetails");


        }

      
    }
}
