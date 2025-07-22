using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Web.ViewModels.Contacts;

namespace Recipify.Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ContactsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("contacts")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Contacts()
        {
            return View(new ContactFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model); 
            }

            var contactMessage = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                SentOn = DateTime.UtcNow
            };

            dbContext.ContactMessages.Add(contactMessage);
            await dbContext.SaveChangesAsync();

            TempData["Success"] = "Your message has been sent successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
