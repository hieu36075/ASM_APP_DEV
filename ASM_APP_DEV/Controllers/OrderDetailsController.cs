using ASM_APP_DEV.Data;
using ASM_APP_DEV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
			var currentUser = await userManager.GetUserAsync(User);
			var orderDetails = new List<OrderDetail>();

   //         if (id != 0)
			//{
			//	 orderDetails = context.OrderDetails.Include(t => t.Order).Include(b => b.Book)
			//				  .Where(t => t.Order.UserId == currentUser.Id).ToList();
			//}
			//else { 
				 orderDetails = context.OrderDetails.Include(t => t.Order).Include(b => b.Book)
					.Where(t => t.Order.OrderStatus == Enums.OrderStatus.Unconfirmed).ToList();
			//}
			ViewBag.User = currentUser;
            return View(orderDetails);

		}
        [HttpGet]

        public IActionResult Delete(int id)
        {
            var OrderDetails = context.OrderDetails.Include(t => t.Order).SingleOrDefault(t => t.Id == id && t.Order.OrderStatus == Enums.OrderStatus.Unconfirmed);
            context.Remove(OrderDetails);
			context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}


