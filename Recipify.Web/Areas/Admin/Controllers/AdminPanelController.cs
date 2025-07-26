using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipify.Web.ViewModels.Admin;

namespace Recipify.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AdminPanelController> logger;
        public AdminPanelController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminPanelController> logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {

            var users = userManager.Users.ToList();

            var model = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Could not create the role.");
                        return RedirectToAction(nameof(Index));
                    }
                }

                var result = await userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    logger.LogInformation("User '{Email}' added to role '{Role}' by '{Admin}' at {Time}.",
                        user.Email,
                        role,
                        User.Identity?.Name ?? "Unknown",
                        DateTime.UtcNow);
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null && await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.RemoveFromRoleAsync(user, role);
                if (result.Succeeded)
                {
                    logger.LogInformation("User '{Email}' removed from role '{Role}' by '{Admin}' at {Time}.",
                        user.Email,
                        role,
                        User.Identity?.Name ?? "Unknown",
                        DateTime.UtcNow);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
