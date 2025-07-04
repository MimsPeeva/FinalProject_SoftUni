using Microsoft.AspNetCore.Mvc;
using Recipify.Data;

namespace Recipify.Web.Controllers
{
    public class RecipifyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public RecipifyController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
