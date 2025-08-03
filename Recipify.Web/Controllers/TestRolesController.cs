using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Recipify.Web.Controllers
{
    public class TestRolesController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TestRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Assign(string email, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Message = $"❌ User '{email}' not found.";
                return View("Index");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                ViewBag.Message = $"❌ Role '{role}' does not exist.";
                return View("Index");
            }

            var result = await userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                ViewBag.Message = $"✅ Role '{role}' added to user '{email}'.";
            }
            else
            {
                ViewBag.Message = $"❌ Failed to add role. {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return View("Index");
        }
    
}
}
