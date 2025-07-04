using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class CuisineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
