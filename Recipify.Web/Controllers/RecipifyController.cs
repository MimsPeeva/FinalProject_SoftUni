using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recipify.Data;
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

        public RecipifyController(
            IRecipeService recipeService,
            ICategoryService categoryService,
            ICuisinesService cuisineService,
            IDifficultyLevelService difficultyService,
            ICommentService commentService,
            UserManager<IdentityUser> userManager)
        {
            this.recipeService = recipeService;
            this.categoryService = categoryService;
            this.cuisineService = cuisineService;
            this.difficultyService = difficultyService;
            this.commentService = commentService;
            this.userManager = userManager;
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
                //var recipe = await recipeService.GetByIdWithCommentsAsync(id);
                //if (recipe == null)
                //{
                //    return RedirectToAction(nameof(Index));
                //}
                //var inputModel = new EditRecipeViewModel
                //{
                //    Id = recipe.Id,
                //    Title = recipe.Title,
                //    ShortDescription = recipe.ShortDescription,
                //    ImageUrl = recipe.ImageUrl,
                //   // CategoryId = recipe.CategoryId,
                //    //Ingredients = recipe.Ingredients.Split(',').ToList(),


                //    Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                //        .Select(c => new SelectListItem
                //        {
                //            Text = c.Text,
                //            Value = c.Text.ToString()
                //        }),

                //    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                //    .Select(c => new SelectListItem
                //    {
                //        Text = c.Text,
                //        Value = c.Text.ToString()
                //    }),

                // DifficultyLevelId = recipe.DifficultyLevelId,
                //    DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                //        .Select(d => new SelectListItem
                //        {
                //            Text = d.Text,
                //            Value = d.Text.ToString()
                //        }),
                //Instructions = recipe.Instructions,
                //Ingredients = recipe.Ingredients.Split(',').ToList() // Assuming Ingredients is a comma-separated string
                // };
               // return View(inputModel);

                var recipe = await recipeService.GetByIdWithCommentsAsync(id);
                if (recipe == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var model = new EditRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title ?? string.Empty,
                    ShortDescription = recipe.ShortDescription ?? string.Empty,
                    ImageUrl = recipe.ImageUrl ?? string.Empty,
                   // Instructions = recipe.Instructions ?? string.Empty,
                    CategoryId = recipe.CategoryId,
                    CuisineId = recipe.CuisineId,
                    DifficultyLevelId = recipe.DifficultyLevelId,
                    Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, // Assuming CategoriesDropdownModel has a property 'Name'
                            Value = c.Id.ToString() // Assuming CategoriesDropdownModel has a property 'Id'
                        }),
                    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name, // Assuming CuisinesDropDownModel has a property 'Name'
                            Value = c.Id.ToString() // Assuming CuisinesDropDownModel has a property 'Id'
                        }),
                    DifficultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem
                        {
                            Text = d.levelName, // Assuming DificultyLevelDropDownModel has a property 'LevelName'
                            Value = d.Id.ToString() // Assuming DificultyLevelDropDownModel has a property 'Id'
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
        public async Task<IActionResult> Edit(EditRecipeViewModel model)
        {
            try
            {
                var recipe = await recipeService.GetByIdWithCommentsAsync(model.Id);
                if (recipe == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var inputModel = new EditRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    ShortDescription = recipe.ShortDescription,
                    ImageUrl = recipe.ImageUrl,
                    CategoryId = recipe.CategoryId,
                    // Ingredients = recipe.Ingredients.Split(',').ToList(),


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
                        .Select(d => new SelectListItem { Text = d.levelName, Value = d.Id.ToString() })
                };
                return View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var recipe = await recipeService.GetRecipesDetailsAsync(id.Value);
                if (recipe == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(recipe);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while loading the recipe for deletion.");
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        // [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await recipeService.DeleteRecipesAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the recipe.");
                return RedirectToAction(nameof(Index));
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