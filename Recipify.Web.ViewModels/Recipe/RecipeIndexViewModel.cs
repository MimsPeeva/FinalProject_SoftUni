using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class RecipeIndexViewModel:RecipeBaseViewModel
    {
        public IEnumerable<string> Categories { get; set; } = new List<string>(); 
        public IEnumerable<string> Cuisines { get; set; } = new List<string>(); 
        public IEnumerable<string> DifficultyLevels { get; set; } = new List<string>();
    }
}
