using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core.Contracts
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeIndexViewModel>> GetAllRecipesAsync();
        Task<DetailsRecipeViewModel> GetRecipesDetailsAsync(int? id);
        Task<DetailsRecipeViewModel> GetByIdWithCommentsAsync(int id);
        Task CreateRecipesAsync(/*string userId,*/ CreateRecipeInputModel model);
        Task EditRecipesAsync(EditRecipeViewModel model);
        Task DeleteRecipesAsync(int id);
        Task<IEnumerable<RecipeIndexViewModel>> SearchRecipesAsync(string query);
        Task<bool> UpdateAsync(EditRecipeViewModel model);
    }
}
