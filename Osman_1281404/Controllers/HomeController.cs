using Microsoft.AspNetCore.Mvc;

namespace Osman_1281404.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
