using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipify.Data.Models;
using Recipify.GCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Ingredient;
namespace Recipify.Data.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.Property(i => i.Name)
              .IsRequired()
              .HasMaxLength(IngredientNameMaxLength);

            builder.Property(i => i.Quantity)
                .HasMaxLength(IngredientQuantityMaxLength);

            builder.HasOne(i => i.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeId);

            builder.HasData(this.GenerateSeedIngredients());
        }

        private List<Ingredient> GenerateSeedIngredients()
        {
            List<Ingredient> seedIngredients = new List<Ingredient>()
            {
                new Ingredient
                {
                    Id = 1,
                    Name = "Spaghetti",
                    Quantity = "200g",
                    RecipeId = 1
                },
                new Ingredient
                {
                    Id = 2,
                    Name = "Eggs",
                    Quantity = "2 large",
                    RecipeId = 1
                },
                new Ingredient
                {
                    Id = 3,
                    Name = "Pancetta",
                    Quantity = "100g",
                    RecipeId = 1
                },
                new Ingredient
                {
                    Id = 4,
                    Name = "Parmesan cheese",
                    Quantity = "50g grated",
                    RecipeId = 1
                },
                new Ingredient
                {
                    Id = 5,
                    Name = "Black pepper",
                    Quantity = "to taste",
                    RecipeId = 1
                },
                new Ingredient
                {
                    Id = 6,
                    Name = "Chicken",
                    Quantity = "500g, cut into pieces",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 7,
                    Name = "Onion",
                    Quantity = "1 large, chopped",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 8,
                    Name = "Garlic",
                    Quantity = "2 cloves, minced",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 9,
                    Name = "Ginger",
                    Quantity = "1 inch, grated",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 10,
                    Name = "Curry powder",
                    Quantity = "2 tablespoons",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 11,
                    Name = "Coconut milk",
                    Quantity = "400ml",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 12,
                    Name = "Salt",
                    Quantity = "to taste",
                    RecipeId = 2
                },
                new Ingredient
                {
                    Id = 13,
                    Name = "Flour",
                    Quantity = "1 cup",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 14,
                    Name = "Sugar",
                    Quantity = "2 tablespoons",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 15,
                    Name = "Baking powder",
                    Quantity = "1 tablespoon",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 16,
                    Name = "Salt",
                    Quantity = "1/2 teaspoon",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 17,
                    Name = "Milk",
                    Quantity = "1 cup",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 18,
                    Name = "Oil",
                    Quantity = "2 tablespoons",
                    RecipeId = 3
                },
                new Ingredient
                {
                    Id = 19,
                    Name = "Egg",
                    Quantity = "1 large",
                    RecipeId = 3
                },
            };


            return seedIngredients;
        }
    }
}
