using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class ContactsController : Controller
    {
        [HttpGet]
        [Route("contacts")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
