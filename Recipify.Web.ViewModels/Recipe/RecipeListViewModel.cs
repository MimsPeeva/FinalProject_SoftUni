using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class RecipeListViewModel : List<RecipeIndexViewModel>
    {
        public IEnumerable<RecipeIndexViewModel> Recipes { get; set; } = new List<RecipeIndexViewModel>();

        public string SearchName { get; set; } = string.Empty;

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}

