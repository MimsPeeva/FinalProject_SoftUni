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
    public class CuisinesService : ICuisinesService
    {
        private readonly ApplicationDbContext dbContext;
        public CuisinesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CuisinesDropDownModel>> GetAllCuisinesDropDownAsync()
        {
            IEnumerable<CuisinesDropDownModel> recipesAsDropDown = await this.dbContext
                   .Recipes
                   .AsNoTracking()
                   .Select(t => new CuisinesDropDownModel()
                   {
                       Id = t.Id,
                       Name = t.Cuisine.Name
                   })
                   .ToArrayAsync();
            return recipesAsDropDown;
        }
    }
   
}
