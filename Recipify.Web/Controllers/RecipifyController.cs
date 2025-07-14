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
                if (id == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }
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
                        .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                        .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }),
                    DificultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() }) // Explicit conversion added here
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
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var recipe = await recipeService.GetByIdWithCommentsAsync(id);
                if (recipe == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var inputModel = new EditRecipeViewModel
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    ShortDescription = recipe.ShortDescription,
                    Instructions = recipe.Instructions,
                    ImageUrl = recipe.ImageUrl,
                    CategoryId = recipe.CategoryId,
                    Ingredients = recipe.Ingredients.Split(',').ToList(),

                   
                    Categories = (await categoryService.GetAllCategoriesDropDownAsync())
                        .Select(c => new SelectListItem
                        {
                            Text = c.Name,
                            Value = c.Id.ToString()
                        }),
                 
                    Cuisines = (await cuisineService.GetAllCuisinesDropDownAsync())
                    .Select(c=>new SelectListItem
                    { Text = c.Name,
                    Value = c.Id.ToString()}),

                    DificultyLevels = (await difficultyService.GetAllDifficultyLevelsDropDownAsync())
                        .Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() }) 
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete (int? id )
    {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                await recipeService.DeleteRecipesAsync(id.Value);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the recipe.");
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
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