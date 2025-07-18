using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Recipe;
namespace Recipify.Web.ViewModels.Recipe
{
    public class CreateRecipeInputModel
    {
        [Required]
        [MinLength(RecipeTitleMinLength)]
        [MaxLength(RecipeTitleMaxLength)]
        public string Title { get; set; } = null!;
        [Required]
        [MinLength(RecipeDescriptionMinLength)]
        [MaxLength(RecipeDescriptionMaxLength)]
        public string ShortDescription { get; set; } = null!;
      
        public string? ImageUrl { get; set; }
        //[Required]

       public string CategoryName { get; set; } = null!;
        [Required]

        public List<string>? Ingredients { get; set; } = new List<string>();
     
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; } = null!;
        public int CuisineId { get; set; }

         public IEnumerable<SelectListItem>? Cuisines { get; set; } = null!;
        public int DifficultyLevelId { get; set; }

        public IEnumerable<SelectListItem>? DificultyLevels { get; set; } = null!;
    }
}
