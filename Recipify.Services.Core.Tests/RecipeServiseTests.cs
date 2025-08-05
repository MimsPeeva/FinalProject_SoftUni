using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Recipify.Services.Core;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Recipe;
using Recipify.Data.Models;
using Recipify.Data.Repository.Interfaces;

namespace Recipify.Services.Core.Tests
{
    public class RecipeServiseTests
    {
        [TestFixture]
        public class RecipeServiceTests
        {
            private Mock<IRecipeRepository> _recipeRepoMock;
            private Mock<ICategoryRepository> _categoryRepoMock;
            private RecipeService _service;

            //    [SetUp]
            //    public void Setup()
            //    {
            //        _recipeRepoMock = new Mock<IRecipeRepository>();
            //        _service = new RecipeService(_recipeRepoMock.Object);
            //    }

            //    [Test]
            //    public async Task GetAllRecipesAsync_ReturnsAllRecipes()
            //    {
            //        _recipeRepoMock.Setup(repo => repo.GetAllAsync())
            //            .ReturnsAsync(new List<Recipe> { new Recipe(), new Recipe() });

            //        var result = await _service.GetAllRecipesAsync();

            //        Assert.That(result.Count(), Is.EqualTo(2));
            //    }

            //    [Test]
            //    public async Task GetRecipesDetailsAsync_ReturnsCorrectModel()
            //    {
            //        var recipe = new Recipe { Id = 1, Title = "Test" };
            //        _recipeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(recipe);

            //        var result = await _service.GetRecipesDetailsAsync(1);

            //        Assert.That(result, Is.Not.Null);
            //        Assert.That(result.Id, Is.EqualTo(1));
            //    }

            //    [Test]
            //    public async Task CreateRecipesAsync_AddsRecipe()
            //    {
            //        var input = new CreateRecipeInputModel { Title = "New Recipe" };

            //        await _service.CreateRecipesAsync(input);

            //        _recipeRepoMock.Verify(r => r.AddAsync(It.IsAny<Recipe>()), Times.Once);
            //    }

            //    [Test]
            //    public async Task EditRecipesAsync_UpdatesRecipe()
            //    {
            //        var input = new EditRecipeViewModel { Id = 1 };

            //        var result = await _service.UpdateAsync(input);

            //        Assert.That(result, Is.True);
            //    }

            //    [Test]
            //    public async Task DeleteRecipesAsync_RemovesRecipe()
            //    {
            //        await _service.DeleteRecipesAsync(1);

            //        _recipeRepoMock.Verify(r => r.Delete(It.IsAny<Recipe>()), Times.Once);
            //    }

            //    [Test]
            //    public async Task SearchRecipesAsync_ReturnsMatchingResults()
            //    {
            //        var recipes = new List<Recipe> {
            //        new Recipe { Title = "Pizza" },
            //        new Recipe { Title = "Pasta" }
            //    };

            //        _recipeRepoMock.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Recipe, bool>>>()))
            //                       .ReturnsAsync(recipes);

            //        var result = await _service.SearchRecipesAsync("p");

            //        Assert.That(result, Is.Not.Empty);
            //    }
            //}
        }
    }
}