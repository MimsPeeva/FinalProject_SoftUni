using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Admin
{
    public class UserRoleViewModel
    {
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();
    }
}
