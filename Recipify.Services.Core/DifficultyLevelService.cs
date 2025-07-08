using Microsoft.EntityFrameworkCore;
using Recipify.Data;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core
{
    public class DifficultyLevelService : IDifficultyLevelService
    {
        private readonly ApplicationDbContext dbContext;
        public DifficultyLevelService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<DificultyLevelDropDownModel>> GetAllDifficultyLevelsDropDownAsync()
        {
            IEnumerable<DificultyLevelDropDownModel> recipesAsDropDown = await this.dbContext
                   .Recipes
                   .AsNoTracking()
                   .Select(t => new DificultyLevelDropDownModel()
                   {
                       Id = t.Id,
                       Name = t.Difficulty.Level
                   })
                   .ToArrayAsync();
            return recipesAsDropDown;
        }
    }
}
