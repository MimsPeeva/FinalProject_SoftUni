using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.GCommon
{
    public class ValidationConstants
    {
        public static class Recipe
        {
            public const int RecipeTitleMinLength = 3;
            public const int RecipeTitleMaxLength = 100;
            public const int RecipeDescriptionMinLength = 10;
            public const int RecipeDescriptionMaxLength = 500;
            public const int RecipeInstructionsMinLength = 50;
            public const int RecipeInstructionsMaxLength = 2000;
        }

        public static class Category
        {
            public const int CategoryNameMinLength = 2;
            public const int CategoryNameMaxLength = 50;
        }

        public static class Ingredient
        {
            public const int IngredientNameMinLength = 2;
            public const int IngredientNameMaxLength = 50;
            public const int IngredientQuantityMinLength = 1;
            public const int IngredientQuantityMaxLength = 30;
        }

        public static class User
        {
            public const int UsernameMinLength = 3;
            public const int UsernameMaxLength = 50;
            public const int EmailMaxLength = 100;
        }

        public static class Comment
        {
            public const int CommentContentMaxLength = 300;
            public const int CommentAuthorMinLength = 2;
            public const int CommentAuthorMaxLength = 50;
        }

        public static class DifficultyLevel
        {
            public const int DifficultyLevelMinLength = 3;
            public const int DifficultyLevelMaxLength = 20;
        }

        public static class Cuisine
        {
            public const int CuisineNameMinLength = 2;
            public const int CuisineNameMaxLength = 50;
        }
    }
}
