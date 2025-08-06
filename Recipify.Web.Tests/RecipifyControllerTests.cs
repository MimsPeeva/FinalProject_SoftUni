using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Moq;
using Nest;
using NUnit.Framework;
using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Services.Core;
using Recipify.Services.Core.Contracts;
using Recipify.Web.Controllers;
using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Recipify.Web.Tests.Controllers
{
    [TestFixture]
    public class RecipifyControllerTests
    {
        private Mock<IRecipeService> recipeServiceMock;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<ICuisinesService> cuisineServiceMock;
        private Mock<IDifficultyLevelService> difficultyServiceMock;
        private Mock<ICommentService> commentServiceMock;
        private Mock<UserManager<IdentityUser>> userManagerMock;
        private ApplicationDbContext dbContext;
        private RecipifyController controller;

        [TearDown]
        public void TearDown()
        {
            controller?.Dispose();
            dbContext?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            recipeServiceMock = new Mock<IRecipeService>();
            categoryServiceMock = new Mock<ICategoryService>();
            cuisineServiceMock = new Mock<ICuisinesService>();
            difficultyServiceMock = new Mock<IDifficultyLevelService>();
            commentServiceMock = new Mock<ICommentService>();

            userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            // Use real in-memory ApplicationDbContext for EF Core compatibility
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            dbContext = new ApplicationDbContext(options);

            controller = new RecipifyController(
                recipeServiceMock.Object,
                categoryServiceMock.Object,
                cuisineServiceMock.Object,
                difficultyServiceMock.Object,
                commentServiceMock.Object,
                userManagerMock.Object,
                dbContext);
        }

        // Index GET
        [Test]
        public async Task Index_ReturnsViewWithPagedResults()
        {
            recipeServiceMock.Setup(r => r.GetAllRecipesAsync())
                .ReturnsAsync(new List<RecipeIndexViewModel>
                {
                    new RecipeIndexViewModel(), new RecipeIndexViewModel(),
                });

            var result = await controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult)?.Model as RecipeSearchViewModel;
            Assert.NotNull(vm);
            Assert.AreEqual(1, vm.CurrentPage);
        }

        [Test]
        public async Task Index_WithSearchName_ReturnsFilteredResults()
        {
            recipeServiceMock.Setup(r => r.SearchRecipesAsync("Cake"))
                .ReturnsAsync(new List<RecipeIndexViewModel>
                {
                    new RecipeIndexViewModel { Title = "Cake" }
                });

            var result = await controller.Index("Cake");

            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult)?.Model as RecipeSearchViewModel;
            Assert.AreEqual("Cake", vm.SearchName);
            Assert.IsTrue(vm.Results.Any(r => r.Title == "Cake"));
        }

        [Test]
        public async Task Index_OnException_ReturnsFallbackViewModel()
        {
            recipeServiceMock.Setup(r => r.GetAllRecipesAsync())
                .ThrowsAsync(new Exception("DB down"));

            var result = await controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult)?.Model as RecipeSearchViewModel;
            Assert.NotNull(vm);
            Assert.IsEmpty(vm.Results);
        }

        // Details GET
        [Test]
        public async Task Details_ReturnsViewIfFound()
        {
            recipeServiceMock.Setup(r => r.GetRecipesDetailsAsync(1))
                .ReturnsAsync(new DetailsRecipeViewModel { Id = 1 });

            var result = await controller.Details(1);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Details_RedirectsIfNotFound()
        {
            recipeServiceMock.Setup(r => r.GetRecipesDetailsAsync(1)).ReturnsAsync((DetailsRecipeViewModel)null);

            var result = await controller.Details(1);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task Details_Exception_RedirectsToIndex()
        {
            recipeServiceMock.Setup(r => r.GetRecipesDetailsAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var result = await controller.Details(1);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        // AddComment POST
        [Test]
        public async Task AddComment_RejectsEmptyComment()
        {
            var result = await controller.AddComment(1, "");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Details", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task AddComment_RejectsIfUserNotLoggedIn()
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            userManagerMock.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns((string)null);

            var result = await controller.AddComment(1, "abc");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Details", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task AddComment_AddsCommentIfValid()
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            userManagerMock.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("user1");

            var result = await controller.AddComment(1, "nice!");

            commentServiceMock.Verify(s => s.AddCommentAsync(It.IsAny<CreateCommentInputModel>(), "user1"), Times.Once);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        // Create GET
        [Test]
        public async Task Create_Get_ReturnsViewWithDropdowns()
        {
            categoryServiceMock.Setup(s => s.GetAllCategoriesDropDownAsync()).ReturnsAsync(new[]
            {
                new CategoriesDropdownModel { Id = 1, Name = "Dessert" }
            });
            cuisineServiceMock.Setup(s => s.GetAllCuisinesDropDownAsync()).ReturnsAsync(new[]
            {
                new CuisinesDropDownModel { Id = 1, Name = "Italian" }
            });
            difficultyServiceMock.Setup(s => s.GetAllDifficultyLevelsDropDownAsync()).ReturnsAsync(new[]
            {
                new DificultyLevelDropDownModel { Id = 1, levelName = "Easy" }
            });

            var result = await controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult).Model as CreateRecipeInputModel;
            Assert.NotNull(vm.Categories);
            Assert.NotNull(vm.Cuisines);
            Assert.NotNull(vm.DifficultyLevels);
        }

        [Test]
        public async Task Create_Get_OnException_RedirectsToIndex()
        {
            categoryServiceMock.Setup(s => s.GetAllCategoriesDropDownAsync()).ThrowsAsync(new Exception("fail"));
            var result = await controller.Create();
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        // Create POST
        [Test]
        public async Task Create_Post_InvalidModelState_ReturnsView()
        {
            controller.ModelState.AddModelError("X", "err");
            categoryServiceMock.Setup(s => s.GetAllCategoriesDropDownAsync()).ReturnsAsync(new[]
            {
                new CategoriesDropdownModel { Id = 1, Name = "Dessert" }
            });
            cuisineServiceMock.Setup(s => s.GetAllCuisinesDropDownAsync()).ReturnsAsync(new[]
            {
                new CuisinesDropDownModel { Id = 1, Name = "Italian" }
            });
            difficultyServiceMock.Setup(s => s.GetAllDifficultyLevelsDropDownAsync()).ReturnsAsync(new[]
            {
                new DificultyLevelDropDownModel { Id = 1, levelName = "Easy" }
            });

            var input = new CreateRecipeInputModel();

            var result = await controller.Create(input);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var input = new CreateRecipeInputModel();
            var result = await controller.Create(input);
            recipeServiceMock.Verify(s => s.CreateRecipesAsync(input), Times.Once);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task Create_Post_OnException_RedirectsToHomeIndex()
        {
            recipeServiceMock.Setup(s => s.CreateRecipesAsync(It.IsAny<CreateRecipeInputModel>())).Throws(new Exception());
            var input = new CreateRecipeInputModel();

            var result = await controller.Create(input);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
            Assert.AreEqual("Home", ((RedirectToActionResult)result).ControllerName);
        }

        // Edit GET
        [Test]
        public async Task Edit_Get_Found_ReturnsView()
        {
            dbContext.Recipes.Add(new Recipe { Id = 1, Title="title",Description="desc" ,Instructions="instructions", Ingredients = new List<Ingredient>() });
            dbContext.SaveChanges();

            categoryServiceMock.Setup(s => s.GetAllCategoriesDropDownAsync()).ReturnsAsync(new[]
            {
                new CategoriesDropdownModel { Id = 1, Name = "Dessert" }
            });
            cuisineServiceMock.Setup(s => s.GetAllCuisinesDropDownAsync()).ReturnsAsync(new[]
            {
                new CuisinesDropDownModel { Id = 1, Name = "Italian" }
            });
            difficultyServiceMock.Setup(s => s.GetAllDifficultyLevelsDropDownAsync()).ReturnsAsync(new[]
            {
                new DificultyLevelDropDownModel { Id = 1, levelName = "Easy" }
            });

            var result = await controller.Edit(1);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Edit_Get_NotFound_RedirectsToIndex()
        {
            // No recipe with id 999 in in-memory db
            var result = await controller.Edit(999);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        // Edit POST
        [Test]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Error", "Invalid");
            categoryServiceMock.Setup(s => s.GetAllCategoriesDropDownAsync()).ReturnsAsync(new[]
            {
                new CategoriesDropdownModel { Id = 1, Name = "Dessert" }
            });
            cuisineServiceMock.Setup(s => s.GetAllCuisinesDropDownAsync()).ReturnsAsync(new[]
            {
                new CuisinesDropDownModel { Id = 1, Name = "Italian" }
            });
            difficultyServiceMock.Setup(s => s.GetAllDifficultyLevelsDropDownAsync()).ReturnsAsync(new[]
            {
                new DificultyLevelDropDownModel { Id = 1, levelName = "Easy" }
            });

            var model = new EditRecipeViewModel();

            var result = await controller.Edit(model, null);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        // Delete GET
        [Test]
        public async Task Delete_Get_Found_ReturnsView()
        {
            recipeServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new DeleteRecipeInputModel { Id = 1 });

            var result = await controller.Delete(1);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Delete_Get_NotFound_ReturnsNotFound()
        {
            recipeServiceMock.Setup(s => s.GetByIdAsync(55)).ReturnsAsync((DeleteRecipeInputModel)null);

            var result = await controller.Delete(55);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        // Delete POST
        [Test]
        public async Task Delete_Post_InvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("Err", "invalid");
            var model = new DeleteRecipeInputModel();
            var result = await controller.Delete(model);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Delete_Post_NotFound_ReturnsNotFound()
        {
            recipeServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DeleteRecipeInputModel)null);
            var model = new DeleteRecipeInputModel { Id = 5 };
            var result = await controller.Delete(model);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        

        [Test]
        public async Task Delete_Post_Exception_ReturnsView()
        {
            recipeServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new DeleteRecipeInputModel { Id = 2 });
            recipeServiceMock.Setup(s => s.SoftDeleteRecipeAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            var model = new DeleteRecipeInputModel { Id = 2 };
            var result = await controller.Delete(model);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        // Search GET
        [Test]
        public async Task Search_EmptyQuery_ReturnsEmptyResults()
        {
            var result = await controller.Search("");
            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult).Model as RecipeSearchViewModel;
            Assert.IsEmpty(vm.Results);
        }

        [Test]
        public async Task Search_ReturnsResults()
        {
            recipeServiceMock.Setup(s => s.SearchRecipesAsync("Cake")).ReturnsAsync(new[]
            {
                new RecipeIndexViewModel { Title = "Cake" }
            });

            var result = await controller.Search("Cake");
            Assert.IsInstanceOf<ViewResult>(result);
            var vm = (result as ViewResult).Model as RecipeSearchViewModel;
            Assert.IsTrue(vm.Results.Any(r => r.Title == "Cake"));
        }

        [Test]
        public async Task Search_Exception_ReturnsRedirect()
        {
            recipeServiceMock.Setup(s => s.SearchRecipesAsync(It.IsAny<string>())).ThrowsAsync(new Exception());
            var result = await controller.Search("Foo");
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Search", ((RedirectToActionResult)result).ActionName);
        }
    }
}