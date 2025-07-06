using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipify.Data.Models;
using Recipify.GCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Recipe;
namespace Recipify.Data.Configuration
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.Property(r => r.Title)
             .IsRequired()
             .HasMaxLength(RecipeTitleMaxLength);

            builder.Property(r => r.Description)
                             .IsRequired()
                .HasMaxLength(RecipeDescriptionMaxLength);

            builder.Property(r => r.Instructions)
                .IsRequired()
                .HasMaxLength(RecipeInstructionsMaxLength);

            builder.Property(e => e.ImageUrl)
           .IsRequired(false);

            builder.HasOne(r => r.Category)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CategoryId);

            builder.HasOne(r => r.Difficulty)
                .WithMany(d => d.Recipes)
                .HasForeignKey(r => r.DifficultyId);

            builder.HasOne(r => r.Cuisine)
                .WithMany(c => c.Recipes)
                .HasForeignKey(r => r.CuisineId);

            //builder.HasData(this.GenerateSeedRecipes());

        }

        private List<Recipe> GenerateSeedRecipes()
        {
            List<Recipe> seedRecipes = new List<Recipe>()
            {
                new Recipe
                {
                    Id = 1,
                    Title = "Spaghetti Carbonara",
                    Description = "A classic Italian pasta dish made with eggs, cheese, pancetta, and pepper.",
                    Instructions = "Cook spaghetti. In a bowl, mix eggs and cheese. Fry pancetta. Combine all with pepper.",
                    CategoryId = 1,
                    DifficultyId = 1,
                    CuisineId = 1
                },
                new Recipe
                {
                    Id = 2,
                    Title = "Chicken Curry",
                    Description = "A spicy and flavorful chicken dish cooked in a rich curry sauce.",
                    Instructions = "Sauté onions, garlic, and ginger. Add spices and chicken. Simmer in coconut milk.",
                    CategoryId = 2,
                    DifficultyId = 2,
                    CuisineId = 2
                },
                 new Recipe
                {
                    Id = 2,
                    Title = "Pancakes",
                    Description = "Easy homemade recipe for thick, fluffy and delicious pancakes.",
                    Instructions = "Combine flour, sugar, baking powder, and salt in a large bowl. Make a well in the center, and pour in milk, oil, and egg. Mix until smooth." +
                    "Heat a lightly oiled griddle or frying pan over medium-high heat. Pour or scoop batter onto the griddle. Cook until bubbles form and the edges are dry, 1 to 2 minutes. Flip and cook until browned on the other side. ",
                    CategoryId = 3,
                    DifficultyId = 1,
                    CuisineId = 9
                }

            };
            return seedRecipes;
        }
    }
}
