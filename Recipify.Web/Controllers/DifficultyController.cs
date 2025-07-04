using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class DifficultyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
