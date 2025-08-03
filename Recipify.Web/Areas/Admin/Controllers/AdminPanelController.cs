using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
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
            var allRoles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

            var model = new List<UserRoleViewModel>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    AllRoles = allRoles
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToRole(string userId, string role)
        {
            logger.LogWarning(">>> AddToRole called with userId={UserId}, role={Role}", userId, role);
            if (string.IsNullOrWhiteSpace(role))
            {
                TempData["Error"] = "Role name is required.";
                return RedirectToAction(nameof(Index));
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                TempData["Error"] = "Role does not exist.";
                return RedirectToAction(nameof(Index));
            }

            var result = await userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{role}' added to user '{user.Email}'.";
            }
            else
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                TempData["Error"] = $"User is not in role '{role}'.";
                return RedirectToAction(nameof(Index));
            }

            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role '{role}' removed from user '{user.Email}'.";
            }
            else
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
