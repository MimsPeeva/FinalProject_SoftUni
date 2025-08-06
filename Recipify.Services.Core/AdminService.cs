using Microsoft.AspNetCore.Identity;
using Recipify.Data.Models;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core
{
    public class AdminService : IAdminService
    {
        //private readonly UserManager<User> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

        //public async Task<IEnumerable<UserRoleViewModel>> GetAllUsersWithRolesAsync()
        //{
        //var users = _userManager.Users.ToList();
        //var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        //var result = new List<UserRoleViewModel>();

        //foreach (var user in users)
        //{
        //    var roles = await _userManager.GetRolesAsync(user);
        //    result.Add(new UserRoleViewModel
        //    {
        //        UserId = user.Id,
        //        Email = user.Email,
        //        Roles = roles.ToList(),
        //        AllRoles = allRoles
        //    });
        //}

        //return result;
        //    }
        //    public async Task<bool> AddUserToRoleAsync(string userId, string role)
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        if (user == null || string.IsNullOrEmpty(role))
        //            return false;

        //        if (!await _roleManager.RoleExistsAsync(role))
        //            return false;

        //        var result = await _userManager.AddToRoleAsync(user, role);
        //        return result.Succeeded;
        //    }



        //    public async Task<bool> RemoveUserFromRoleAsync(string userId, string role)
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        if (user == null || string.IsNullOrEmpty(role))
        //            return false;

        //        if (!await _roleManager.RoleExistsAsync(role))
        //            return false;

        //        var result = await _userManager.RemoveFromRoleAsync(user, role);
        //        return result.Succeeded;
        //    }
        //}
    }
}
