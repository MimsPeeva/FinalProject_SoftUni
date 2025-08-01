using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class RecipeSearchViewModel
    {
        public string SearchName { get; set; }
       // public List<SelectListItem> Categories { get; set; }
        public List<RecipeIndexViewModel> Results { get; set; } = new List<RecipeIndexViewModel>();
    }


    }
