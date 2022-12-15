using Microsoft.AspNetCore.Mvc;

namespace ASM_APP_DEV.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
    }
}
