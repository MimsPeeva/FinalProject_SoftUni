using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class RecipifyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
