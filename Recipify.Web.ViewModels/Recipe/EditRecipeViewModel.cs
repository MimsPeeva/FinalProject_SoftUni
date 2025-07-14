using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class EditRecipeViewModel:CreateRecipeInputModel
    {
        public int Id { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public IEnumerable<SelectListItem> DifficultyLevels { get; set; }
    }
  
}
