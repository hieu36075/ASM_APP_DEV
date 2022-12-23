using ASM_APP_DEV.Data;
using ASM_APP_DEV.Enums;
using ASM_APP_DEV.Models;
using ASM_APP_DEV.ViewModel;
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
			var viewModelCart = new ViewModelCart();
			var currentUser = await userManager.GetUserAsync(User);

   //         if (id != 0)
			//{
			//	 orderDetails = context.OrderDetails.Include(t => t.Order).Include(b => b.Book)
			//				  .Where(t => t.Order.UserId == currentUser.Id).ToList();
			//}
			//else { 
			var	 orderDetails = context.OrderDetails.Include(t => t.Order)
					.Where(t => t.Order.OrderStatus == Enums.OrderStatus.Unconfirmed).ToList();
			//}
			foreach(var item in orderDetails)
			{
				item.Book = context.Books.SingleOrDefault(b => b.Id == item.IdBook);
			}
			

			viewModelCart.Address = currentUser.Address;
			viewModelCart.FullName = currentUser.FullName;
			viewModelCart.Phone = currentUser.PhoneNumber;
			viewModelCart.OrderDetails = orderDetails;
			viewModelCart.Order = orderDetails[0].Order;
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

		public IActionResult BuyBooks()
		{

			return View();
		}
    }
}


