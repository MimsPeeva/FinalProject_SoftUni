namespace Recipify.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Recipify.Web.ViewModels.Contacts;
    using System.Diagnostics;
    using ViewModels;

    public class HomeController : Controller
    {
        public HomeController(ILogger<HomeController> logger)
        {

        }

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

        [HttpGet]
        public IActionResult Contacts()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        public IActionResult SendMessage(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Contacts", model);
            }

            // TODO: Add logic to save message to DB or send email
            TempData["SuccessMessage"] = "Your message has been sent successfully!";
            return RedirectToAction("Contacts");
        }
    }
}
