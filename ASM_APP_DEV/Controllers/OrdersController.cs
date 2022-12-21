using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		public IActionResult Index()
        {
			var oderInDb = context.Orders.ToList();
            return View(oderInDb);

        }
        [HttpGet]
		public async Task<IActionResult> Create(int id) {

            var currentUser = await userManager.GetUserAsync(User);


            Book bookInDB = context.Books.SingleOrDefault(t => t.Id == id);

                var order = new Order();
                order.UserId = currentUser.Id;
                order.OrderStatus = Enums.OrderStatus.Unconfirmed;
                order.PriceOrder = 0;
                context.Add(order);
                context.SaveChanges();

                OrderDetail orderDetail = new OrderDetail();
                orderDetail.IdBook = bookInDB.Id;
                orderDetail.IdOrder = order.Id;
                orderDetail.Quantity = 1;
                orderDetail.Price = 0;
            orderDetail.Order = order;
            orderDetail.Book = bookInDB; 
     
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
                order.OrderDetails.Add(orderDetail);
            //}
            //else
            //{

            //    OrderDetail orderDetail = new OrderDetail();
            //    orderDetail.IdBook = bookInDB.Id;
            //    orderDetail.IdOrder = orderUnconfirmed.Id;
            //    orderDetail.Quantity = 1;
            //    orderUnconfirmed.OrderDetails.Add(orderDetail);
            //}
            context.SaveChanges();

            return RedirectToAction("Index", "OrderDetails");


        }

      
    }
}
