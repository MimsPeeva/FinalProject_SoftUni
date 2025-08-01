﻿using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Required]
        [MinLength(RecipeInstructionsMinLength)]
        [MaxLength(RecipeInstructionsMaxLength)]
        public string Instructions { get; set; } = null!;
        public string? ImageUrl { get; set; }
        //[Required]

     //  public string CategoryName { get; set; } = null!;
        [Required]

       public List<IngredientInputModel>? Ingredients { get; set; } = new List<IngredientInputModel>();
     
        public int Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public int Cuisine { get; set; }

         public IEnumerable<SelectListItem> Cuisines { get; set; } = new List<SelectListItem>();
        public int DifficultyLevel { get; set; }

        public IEnumerable<SelectListItem>? DifficultyLevels { get; set; } = new List<SelectListItem>();
    }
}
