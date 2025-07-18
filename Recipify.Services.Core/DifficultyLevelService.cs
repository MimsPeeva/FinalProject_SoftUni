using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IEnumerable<SelectListItem>> GetAllDifficultyLevelsDropDownAsync()
        {

            return await this.dbContext
           .Difficulties
           .AsNoTracking()
           .Select(c => new SelectListItem
           {
               Value = c.Id.ToString(),
               Text = c.Level
           })
           .ToListAsync();
            //{
            //    return await dbContext
            //.Difficulties
            //.AsNoTracking()
            //.Select(c => new DificultyLevelDropDownModel
            //{
            //    Id = c.Id,
            //    Name = c.Level
            //})
            //.ToListAsync();
        }
    }
}
