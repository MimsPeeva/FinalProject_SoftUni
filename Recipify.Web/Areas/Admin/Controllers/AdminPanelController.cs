﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
                var allRoles = roleManager.Roles.Select(r => r.Name).ToList();

                model.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    AllRoles =  allRoles
                });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (string.IsNullOrWhiteSpace(role))
            {
                logger.LogWarning("Role name was empty for user ID: {UserId}", userId);
                return RedirectToAction(nameof(Index));
            }

            if (user == null)
            {
                logger.LogWarning("User not found with ID: {UserId}", userId);
                return RedirectToAction(nameof(Index));
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                logger.LogWarning("Role '{Role}' does not exist", role);
                return RedirectToAction(nameof(Index));
            }

            var result = await userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                logger.LogInformation("Added role '{Role}' to user '{Email}'", role, user.Email);
            }
            else
            {
                logger.LogError("Failed to add role. Errors: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
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
            return RedirectToAction(nameof(Index), new { message = "Role removed successfully." });
        }
    }
}
