using Microsoft.AspNetCore.Mvc;

namespace OnlineShopping.WebApp.Controllers
{
    public class CartTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
