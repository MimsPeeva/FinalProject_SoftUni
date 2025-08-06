using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Web.ViewModels.Recipe;
namespace Recipify.Services.Core.Tests
{
    [TestFixture]
    public class RecipeServiseTests
    {
        private ApplicationDbContext _dbContext;
        private RecipeService _service;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
       .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
       .Options;

            _dbContext = new ApplicationDbContext(options);
            _service = new RecipeService(_dbContext);

            _dbContext.Categories.Add(new Category { Id = 1, Name = "Dessert" });
            _dbContext.Cuisines.Add(new Cuisine { Id = 1, Name = "Italian" });
            _dbContext.Difficulties.Add(new DifficultyLevel { Id = 1, Level = "Easy" });
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetAllRecipesAsync_Returns_All_Recipes()
        {
            _dbContext.Recipes.Add(new Recipe { Id = 1, Title = "Recipe1",Description="desc",Instructions="instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1 });
            _dbContext.Recipes.Add(new Recipe { Id = 2, Title = "Recipe2", Description = "desc", Instructions = "instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1 });
            _dbContext.SaveChanges();

            var result = await _service.GetAllRecipesAsync();

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetRecipesDetailsAsync_Returns_Correct_Recipe()
        {
            var recipe = new Recipe
            {
                Id = 10,
                Title = "DetailsTest",
                Description = "desc",
                Instructions = "instructions",
                CategoryId = 1,
                CuisineId = 1,
                DifficultyId = 1,
                Category = _dbContext.Categories.First(),
                Cuisine = _dbContext.Cuisines.First(),
                Difficulty = _dbContext.Difficulties.First()
            };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var result = await _service.GetRecipesDetailsAsync(10);

            Assert.NotNull(result);
            Assert.AreEqual("DetailsTest", result.Title);
        }

        [Test]
        public async Task GetRecipesDetailsAsync_NullId_ReturnsNull()
        {
            var result = await _service.GetRecipesDetailsAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdWithCommentsAsync_Returns_Recipe()
        {
            var recipe = new Recipe
            {
                Id = 11,
                Title = "WithComments",
                Description = "desc",
                Instructions = "instructions",
                CategoryId = 1,
                CuisineId = 1,
                DifficultyId = 1,
                Category = _dbContext.Categories.First(),
                Cuisine = _dbContext.Cuisines.First(),
                Difficulty = _dbContext.Difficulties.First()
            };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var result = await _service.GetByIdWithCommentsAsync(11);

            Assert.NotNull(result);
            Assert.AreEqual("WithComments", result.Title);
        }

        [Test]
        public async Task CreateRecipesAsync_Adds_Recipe()
        {
            var model = new CreateRecipeInputModel
            {
                Title = "Tiramisu",
                ShortDescription = "Classic Italian dessert",
                Instructions = "Mix, layer, chill.",
                ImageUrl = "http://example.com/tiramisu.jpg",
                Category = 1,        
                Cuisine = 1,         
                DifficultyLevel = 1,  
                Ingredients = new List<IngredientInputModel>
                {
                    new IngredientInputModel { Name = "Mascarpone", Quantity = "500g" },
                    new IngredientInputModel { Name = "Eggs", Quantity = "3" },
                }
            };

            await _service.CreateRecipesAsync(model);

            var recipe = await _dbContext.Recipes.FirstOrDefaultAsync(r => r.Title == "Tiramisu");
            Assert.IsNotNull(recipe);
            Assert.AreEqual("Classic Italian dessert", recipe.Description);
            Assert.AreEqual(1, recipe.CategoryId);
            Assert.AreEqual(1, recipe.CuisineId);
            Assert.AreEqual(1, recipe.DifficultyId);
        }

        [Test]
        public void CreateRecipesAsync_InvalidCategory_Throws()
        {
            var model = new CreateRecipeInputModel
            {

                Title = "Invalid",
                ShortDescription = "desc",
                Instructions = "instr",
                ImageUrl = "img",
                Ingredients = new List<IngredientInputModel>{
                    new IngredientInputModel { Name = "Sugar", Quantity = "1 cup" } },
                Category = 78,
                Cuisine = 1,
                DifficultyLevel = 1
            };
            Assert.ThrowsAsync<ArgumentException>(() => _service.CreateRecipesAsync(model));
        }

        [Test]
        public async Task EditRecipesAsync_Updates_Recipe_And_Ingredients()
        {
            var recipe = new Recipe
            {
                Id = 20,
                Title = "ToEdit",
                Description = "Desc",
                Instructions = "instructions",
                CategoryId = 1,
                CuisineId = 1,
                DifficultyId = 1,
                Ingredients = new List<Ingredient>
                {
                    new() { Id = 1, Name = "Old", Quantity = "1" }
                }
            };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var model = new EditRecipeViewModel
            {
                Id = 20,
                Title = "Edited",
                ShortDescription = "EditedDesc",
                ImageUrl = "url",
                Instructions = "new instructions",
                Category = 1,
                Cuisine = 1,
                DifficultyLevel = 1,
                Ingredients = new List<IngredientInputModel>
                {
                    new() { Id = 1, Name = "Old", Quantity = "2" },
                    new() { Id = 0, Name = "New", Quantity = "3" }
                }
            };

            await _service.EditRecipesAsync(model, null);

            var edited = _dbContext.Recipes.Include(r => r.Ingredients).First(r => r.Id == 20);
            Assert.That(edited.Title, Is.EqualTo("Edited"));
            Assert.That(edited.Ingredients.Count, Is.EqualTo(2));
        }

        [Test]
        public void EditRecipesAsync_RecipeNotFound_Throws()
        {
            var model = new EditRecipeViewModel
            {
                Id = 999,
                Ingredients = new List<IngredientInputModel>()
            };
            Assert.ThrowsAsync<ArgumentException>(() => _service.EditRecipesAsync(model, null));
        }

        [Test]
        public async Task DeleteRecipesAsync_Removes_Recipe()
        {
            var recipe = new Recipe {
                Id = 30,
                Title = "DelRecipe",
                Description="desc",
                Instructions="fffff", 
                CategoryId = 1, CuisineId = 1, DifficultyId = 1 };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            await _service.DeleteRecipesAsync(30);

            var deleted = _dbContext.Recipes.FirstOrDefault(r => r.Id == 30);
            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task SearchRecipesAsync_Returns_Matching()
        {
            _dbContext.Recipes.Add(new Recipe { Id = 40, Title = "Pizza", Description = "desc", Instructions = "instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1 });
            _dbContext.Recipes.Add(new Recipe { Id = 41, Title = "Pasta", Description = "desc", Instructions = "instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1 });
            _dbContext.SaveChanges();

            var result = await _service.SearchRecipesAsync("Piz");

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Pizza", result.First().Title);
        }

        [Test]
        public async Task SearchRecipesAsync_EmptyQuery_Returns_Empty()
        {
            var result = await _service.SearchRecipesAsync("");
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task UpdateAsync_RecipeExists_Returns_True()
        {
            var recipe = new Recipe { Id = 50, Title = "ToUpdate", Description = "desc", Instructions = "instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1 };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var model = new EditRecipeViewModel
            {
                Id = 50,
                Title = "Updated",
                ShortDescription = "Upd",
                ImageUrl = "img",
                Instructions = "instr",
                Category = 1,
                Cuisine = 1,
                DifficultyLevel = 1
            };

            var result = await _service.UpdateAsync(model);

            Assert.IsTrue(result);
            Assert.AreEqual("Updated", _dbContext.Recipes.First(r => r.Id == 50).Title);
        }

        [Test]
        public async Task UpdateAsync_RecipeNotFound_Returns_False()
        {
            var model = new EditRecipeViewModel
            {
                Id = 404,
                Title = "NotFound"
            };
            var result = await _service.UpdateAsync(model);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteRecipeAsync_Deletes_Recipe()
        {
            _dbContext.Categories.Add(new Category { Id = 2, Name = "Dessert" });
            _dbContext.Cuisines.Add(new Cuisine { Id = 2, Name = "Italian" });
            _dbContext.Difficulties.Add(new DifficultyLevel { Id = 2, Level = "Easy" });
            _dbContext.SaveChanges();

            var recipe = new Recipe
            {
                Id = 1,
                Title = "Test Recipe",
                Description = "desc",
                Instructions = "instr",
                CategoryId = 2,
                CuisineId = 2,
                DifficultyId = 2,
                IsDeleted = false
            };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var result = await _service.SoftDeleteRecipeAsync(1);

            Assert.That(result, Is.True);

            var updated = _dbContext.Recipes.IgnoreQueryFilters().First(r => r.Id == 1);
            Assert.That(updated.IsDeleted, Is.True);
        }

        [Test]
        public async Task SoftDeleteRecipeAsync_AlreadyDeleted_ReturnsFalse()
        {
            var recipe = new Recipe { Id = 61, Title = "Soft", Description = "desc", Instructions = "instructions", CategoryId = 1, CuisineId = 1, DifficultyId = 1, IsDeleted = true };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var result = await _service.SoftDeleteRecipeAsync(61);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetByIdAsync_Returns_Recipe()
        {
            var recipe = new Recipe
            {
                Id = 7,
                Title = "ForDelete",
                Description = "desc",
                Instructions = "instructions",
                CategoryId = 1,
                CuisineId = 1,
                DifficultyId = 1,
                Category = _dbContext.Categories.First(),
                Cuisine = _dbContext.Cuisines.First(),
                Difficulty = _dbContext.Difficulties.First()
            };
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();

            var result = await _service.GetByIdAsync(7);
            Assert.NotNull(result);
            Assert.AreEqual("ForDelete", result.Title);
        }

        [Test]
        public async Task GetByIdAsync_NonExisting_ReturnsNull()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.IsNull(result);
        }
    }
    }