using ASM_APP_DEV.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASM_APP_DEV.Controllers
{
    public class OrdersController : Controller
    {
		private ApplicationDbContext context;

		public OrdersController(ApplicationDbContext context)
		{
			this.context = context;
		}
		public IActionResult Index()
        {
			var oderInDb = context.Orders.ToList();
            return View(oderInDb);

        }
    }
}
