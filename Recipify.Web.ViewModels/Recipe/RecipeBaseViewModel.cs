using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class RecipeBaseViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;
        public List<IngredientInputModel> Ingredients { get; set; } = new List<IngredientInputModel>();
        public string? ImageUrl { get; set; }

        public string Comment { get; set; }
        public int CategoryId { get; set; }

        public int CuisineId { get; set; }

        public int DifficultyLevelId { get; set; }
    }
}
