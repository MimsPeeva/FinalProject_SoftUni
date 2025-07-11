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
        public IEnumerable<CategoriesDropdownModel> Categories { get; set; }

        public IEnumerable<CuisinesDropDownModel> Cuisines { get; set; }

        public IEnumerable<DificultyLevelDropDownModel> DifficultyLevels { get; set; }
    }
  
}
