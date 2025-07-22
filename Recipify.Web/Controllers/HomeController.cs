namespace Recipify.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipify.Data;
    using Recipify.Data.Models;
    using Recipify.Web.ViewModels.Contacts;
    using System.Diagnostics;
    using ViewModels;

    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     
    }
}
