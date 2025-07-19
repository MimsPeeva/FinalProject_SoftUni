using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        public RecipeService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        public async Task<IEnumerable<RecipeIndexViewModel>> GetAllRecipesAsync()
        {
            return await dbContext.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Cuisine)
                .Include(r => r.Difficulty)
                .Include(r => r.Ingredients)
                .Include(r => r.Comments)
                  .Select(r => new RecipeIndexViewModel
                  {
                      Id = r.Id,
                      Title = r.Title,
                      CategoryId = r.Category.Id,
                      CuisineId = r.Cuisine.Id,
                        ShortDescription = r.Description,
                        DifficultyLevelId = r.Difficulty.Id,
                        //Comments

                  })
                  .ToListAsync();
        }

        public async Task<DetailsRecipeViewModel> GetRecipesDetailsAsync(int? id)
        {
            if (!id.HasValue)
            {
                return null;
            }

            var recipeModel = await dbContext.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Cuisine)
                .Include(r => r.Difficulty)
                .SingleOrDefaultAsync(r => r.Id == id.Value);

            if (recipeModel == null)
            {
                return null;
            }

            var recipeDetails = new DetailsRecipeViewModel
            {
                Id = recipeModel.Id,
                Title = recipeModel.Title,
                ShortDescription = recipeModel.Description,
                Instructions = recipeModel.Instructions,
                CategoryName = recipeModel.Category?.Name,
                CuisineName = recipeModel.Cuisine?.Name,
                DifficultyLevel = recipeModel.Difficulty?.Level,
                CategoryId = recipeModel.CategoryId,
                CuisineId = recipeModel.CuisineId,
                DifficultyLevelId = recipeModel.DifficultyId,
                Comments = new List<CommentViewModel>() 
            };

            return recipeDetails;
        }
                
          
        
        public async Task<DetailsRecipeViewModel> GetByIdWithCommentsAsync(int id)
        {
            var recipe = await dbContext.Recipes
        .Where(r => r.Id == id)
        .Select(r => new DetailsRecipeViewModel
        {
            Id = r.Id,
            Title = r.Title,
            ShortDescription = r.Description,
            Instructions = r.Instructions,
            CategoryName = r.Category.Name,
            CuisineName = r.Cuisine.Name,
            DifficultyLevel = r.Difficulty.Level,
            Comments = r.Comments.Select(c => new CommentViewModel
            {
                Id = c.Id,
                Content = c.Content,
                AuthorName = c.Author,
            }).ToList()
        })
        .FirstOrDefaultAsync();

            return recipe;
        }

        public async Task CreateRecipesAsync(/*string userId,*/ CreateRecipeInputModel model)
        {
          //  IdentityUser? user = await this.userManager.FindByIdAsync(userId);
            Category? categoryRef = await this.dbContext.Categories.FirstAsync(c => c.Id == model.CategoryId);
            Cuisine? cuisineRef = await this.dbContext.Cuisines.FirstAsync(c => c.Id == model.CuisineId);
            DifficultyLevel? difficultyRef = await this.dbContext.Difficulties.FirstAsync(c => c.Id == model.DifficultyLevelId);
            if (categoryRef != null && cuisineRef != null && difficultyRef != null /*&& user!=null*/)
            {
                Recipe recipe = new Recipe
                {
                    Title = model.Title,
                    Description = model.ShortDescription,
                    Instructions=model.Instructions,
                    ImageUrl = model.ImageUrl,
                    CategoryId = model.CategoryId,
                    CuisineId = model.CuisineId,
                    DifficultyId = model.DifficultyLevelId,
                    Category = categoryRef,
                    Cuisine = cuisineRef,
                    Difficulty = difficultyRef
                };
               await dbContext.Recipes.AddAsync(recipe);
              await  dbContext.SaveChangesAsync();
            }

            throw new ArgumentException("Invalid category, cuisine or difficulty level.");

            //var ingredientList = (model.Ingredients ?? new List<string>())
            //    .Where(i => !string.IsNullOrWhiteSpace(i))
            //    .Select(name => new Ingredient { Name = name.Trim() })
            //    .ToList();

            //  recipe.Ingredients = ingredientList;

        }
        public async Task EditRecipesAsync(EditRecipeViewModel model)
        {
            var recipe = await dbContext.Recipes.FindAsync(model.Id);

            if (recipe == null) return;

            recipe.Title = model.Title;
            recipe.Description = model.ShortDescription;
            recipe.CategoryId = model.CategoryId;
            recipe.CuisineId = model.CuisineId;
            recipe.DifficultyId = model.DifficultyLevelId;

            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteRecipesAsync(int id)
        {
            var recipe = await dbContext.Recipes.FindAsync(id);

            if (recipe != null)
            {
                dbContext.Recipes.Remove(recipe);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RecipeIndexViewModel>> SearchRecipesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<RecipeIndexViewModel>();
            }

            query = query.Trim().ToLower();

            return await dbContext.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Cuisine)
                .Include(r => r.Difficulty)
                .Where(r =>
                    r.Title.ToLower().Contains(query) ||
                    r.Description.ToLower().Contains(query) ||
                    r.Category.Name.ToLower().Contains(query) ||
                    r.Cuisine.Name.ToLower().Contains(query) ||
                    r.Difficulty.Level.ToLower().Contains(query)
                )
                .Select(r => new RecipeIndexViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    CategoryId = r.Category.Id,
                    CuisineId = r.Cuisine.Id,
                    ShortDescription = r.Description,
                    DifficultyLevelId = r.Difficulty.Id
                })
                .ToListAsync();
        }

        //public Task<DetailsRecipeViewModel> GetRecipesDetailsAsync(int id)
        //{
        //}
    }
}
