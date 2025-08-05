using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Repository.Interfaces
{
    public interface IRecipeRepository: IRepository<Recipe,Guid>, IAsyncRepository<Recipe, Guid>
    {
        //Task<IEnumerable<Recipe>> GetByCuisineIdAsync(Guid cuisineId);
        //Task<IEnumerable<Recipe>> GetByDifficultyLevelIdAsync(int difficultyLevelId);
        //Task<IEnumerable<Recipe>> GetByIngredientIdAsync(int ingredientId);
        //Task<IEnumerable<Recipe>> GetByAuthorIdAsync(Guid authorId);
        //Task<IEnumerable<Recipe>> SearchRecipesAsync(string searchTerm, int pageNumber, int pageSize);
        //Task<int> GetTotalRecipesCountAsync(string searchTerm);
    }
}
