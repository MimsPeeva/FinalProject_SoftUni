using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Recipe;

namespace Recipify.Web.Controllers
{
    public class RecipifyController : Controller
    {
        private readonly IRecipeService recipeService;
        private readonly ICategoryService categoryService;
        private readonly ICuisinesService cuisineService;
        private readonly IDifficultyLevelService difficultyService;
        private readonly ICommentService commentService;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext dbContext;
        public RecipifyController(
            IRecipeService recipeService,
            ICategoryService categoryService,
            ICuisinesService cuisineService,
            IDifficultyLevelService difficultyService,
            ICommentService commentService,
            UserManager<IdentityUser> userManager,ApplicationDbContext dbContext)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.cuisineService = cuisineService;
            this.difficultyService = difficultyService;
            this.commentService = commentService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<RecipeIndexViewModel> allRecipes =
                    await recipeService.GetAllRecipesAsync();
                return View(allRecipes);
            }
            catch (Exception )
            {
                ModelState.AddModelError(string.Empty, "An error occurred while loading recipes.");
                return View(new List<RecipeIndexViewModel>());
                //Console.WriteLine(e);
                //return this.RedirectToAction(nameof(Index), "Home");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            try
          
            { 
                DetailsRecipeViewModel? recipeDetails = await recipeService.GetRecipesDetailsAsync(id.Value);
                if (recipeDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
                return this.View(recipeDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
      
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(int recipeId, string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    ModelState.AddModelError(string.Empty, "Comment content cannot be empty.");
                    return RedirectToAction(nameof(Details), new { id = recipeId });
                }

                var userId = userManager.GetUserId(User);
                if (userId == null)
                {
                    ModelState.AddModelError(string.Empty, "You must be logged in to comment.");
                    return RedirectToAction(nameof(Details), new { id = recipeId });
                }

                CreateCommentInputModel? input = new CreateCommentInputModel
                {
                    RecipeId = recipeId,
                    Content = content
                };

                await commentService.AddCommentAsync(input, userId);
                return RedirectToAction(nameof(Details), new { id = recipeId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "An error occurred while adding the comment.");
                return RedirectToAction(nameof(Details), new { id = recipeId });
            }
        }

     
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            try
            {
                CreateRecipeInputModel inputModel = new CreateRecipeInputModel
                {
                    Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, 
                            Value = c.Id.ToString()
                        }),
                    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, 
                            Value = c.Id.ToString() 
                        }),
                    DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem
                        {
                            Text = d.levelName, 
                            Value = d.Id.ToString() 
                        })
                };
                return View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRecipeInputModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                        .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                    model.Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                        .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                    model.DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem { Text = d.levelName, Value = d.Id.ToString() });

                    return View(model);
                }

                await recipeService.CreateRecipesAsync(model);
              //  recipeService.();                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the recipe.");
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {

                var recipe = await dbContext
                    .Recipes
                    .Include(r => r.Ingredients)
                    .FirstOrDefaultAsync(r => r.Id == id);

             //   var recipe = await recipeService.GetByIdWithCommentsAsync(id);
                if (recipe == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var model = new EditRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title ?? string.Empty,
                    ShortDescription = recipe.Description ?? string.Empty,
                    ImageUrl = recipe.ImageUrl ?? string.Empty,
                   Instructions = recipe.Instructions ?? string.Empty,
                   Ingredients=recipe.Ingredients.Select(i => new IngredientInputModel
                    {
                       Id = i.Id,
                       Name = i.Name ?? string.Empty,
                        Quantity = i.Quantity ?? string.Empty,
                    }).ToList(),
                    CategoryId = recipe.CategoryId,
                    CuisineId = recipe.CuisineId,
                    DifficultyLevelId = recipe.DifficultyId,
                    Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, 
                            Value = c.Id.ToString() 
                        }),
                    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, 
                            Value = c.Id.ToString() 
                        }),
                    DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem
                        {
                            Text = d.levelName, 
                            Value = d.Id.ToString() 
                        })
                };

                return View(model);
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditRecipeViewModel model, string? DeletedIngredientIds)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    });

                model.Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    });

                model.DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                    .Select(d => new SelectListItem
                    {
                        Text = d.levelName,
                        Value = d.Id.ToString()
                    });

                return View(model);
            }

            var recipe = await dbContext
                .Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == model.Id);
            if (recipe == null)
            {
                return NotFound();
            }

            recipe.Title = model.Title;
            recipe.Description = model.ShortDescription;
            recipe.ImageUrl = model.ImageUrl;
            recipe.Instructions = model.Instructions;
            recipe.CategoryId = model.CategoryId;
            recipe.CuisineId = model.CuisineId;
            recipe.DifficultyId = model.DifficultyLevelId;

            //var modelIngredientNames = model.Ingredients.Select(i => i.Name).ToList();


            //foreach (var input in model.Ingredients)
            //{
            //    var existing = recipe.Ingredients.FirstOrDefault(i => i.Name == input.Name);
            //    if (existing != null)
            //    {
            //        existing.Quantity = input.Quantity;
            //    }
            //    else
            //    {
            //        recipe.Ingredients.Add(new Ingredient
            //        {
            //            Name = input.Name,
            //            Quantity = input.Quantity
            //        });
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(DeletedIngredientIds))
            {
                var idsToRemove = DeletedIngredientIds
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                var toDelete = recipe.Ingredients
                    .Where(i => idsToRemove.Contains(i.Id))
                    .ToList();

                dbContext.Ingredients.RemoveRange(toDelete);
            }

            foreach (var input in model.Ingredients)
            {
                var existing = recipe.Ingredients.FirstOrDefault(i => i.Id == input.Id);
                if (existing != null)
                {
                    existing.Name = input.Name;
                    existing.Quantity = input.Quantity;
                }
                else
                {
                    recipe.Ingredients.Add(new Ingredient
                    {
                        Name = input.Name,
                        Quantity = input.Quantity,
                        RecipeId = recipe.Id 
                    });
                }
            }


            try
            {
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong while saving.");
                Console.WriteLine(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {

            //    var recipe = await dbContext.Recipes
            //.Include(r => r.Category)
            //.FirstOrDefaultAsync(r => r.Id == id);

            //    if (recipe == null)
            //    {
            //        return NotFound();
            //    }

            //    var viewModel = new DeleteRecipeInputModel
            //    {
            //        Id = recipe.Id,
            //        Title = recipe.Title,
            //        ShortDescription = recipe.Description,
            //        Instructions = recipe.Instructions,
            //        ImageUrl = recipe.ImageUrl,
            //        Category = recipe.Category.Name ?? "Unknown",
            //        Cuisine = recipe.Cuisine?.Name ?? "Unknown",
            //        DifficultyLevel = recipe.Difficulty?.Level ?? "Unknown"
            //    };

            //    return View(viewModel);
            var recipe = await recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var model = new DeleteRecipeInputModel
            {
                Id = recipe.Id,
                Title = recipe.Title,
                ShortDescription = recipe.ShortDescription,
                Instructions = recipe.Instructions,
                ImageUrl = recipe.ImageUrl,
                Category = recipe.Category,            
                Cuisine = recipe.Cuisine,              
                DifficultyLevel = recipe.DifficultyLevel 
            };

            return View(model);
        }
        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(DeleteRecipeInputModel inputModel)
        {
            //try
            //{
            //    await recipeService.DeleteRecipesAsync(id);
            //    return RedirectToAction(nameof(Index));
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    ModelState.AddModelError(string.Empty, "An error occurred while deleting the recipe.");
            //    return RedirectToAction(nameof(Index));
            //}
            if (!ModelState.IsValid)
            {
                return View("Delete", inputModel);
            }

            var recipe = await recipeService.GetByIdAsync(inputModel.Id);
            if (recipe == null)
            {
                return NotFound();
            }

            try
            {
                await recipeService.SoftDeleteRecipeAsync(inputModel.Id);
                TempData["SuccessMessage"] = "Recipe deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the recipe.");
                Console.WriteLine(ex.Message);
                return View("Delete", inputModel);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return RedirectToAction(nameof(Index));
                }
                IEnumerable<RecipeIndexViewModel> searchResults = await recipeService.SearchRecipesAsync(query);
                return View("Index", searchResults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while searching for recipes.");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}