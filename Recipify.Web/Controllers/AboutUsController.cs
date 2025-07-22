using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class AboutUsController : Controller
    {
        [HttpGet]
        [Route("about")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
