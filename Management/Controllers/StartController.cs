using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
