using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
