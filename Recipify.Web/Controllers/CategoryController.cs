using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
