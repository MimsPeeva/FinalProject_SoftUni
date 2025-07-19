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
    public class CuisinesService : ICuisinesService
    {
        private readonly ApplicationDbContext dbContext;
        public CuisinesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CuisinesDropDownModel>> GetAllCuisinesDropDownAsync()
        {

            //     return await this.dbContext
            //.Cuisines
            //.AsNoTracking()
            //.Select(c => new SelectListItem
            //{
            //    Value = c.Id.ToString(),
            //    Text = c.Name
            //})
            //.ToListAsync();
            IEnumerable<CuisinesDropDownModel> cuisinesAsDropDown = await this.dbContext
               .Cuisines
               .AsNoTracking()
               .Select(c => new CuisinesDropDownModel
               {
                   Id = c.Id,
                   Name = c.Name
               })
               .ToArrayAsync();

            return cuisinesAsDropDown;


            //    return await dbContext
            //.Cuisines
            //.AsNoTracking()
            //.Select(c => new CuisinesDropDownModel
            //{
            //    Id = c.Id,
            //    Name = c.Name
            //})
            //.ToListAsync();
        }
    }
   
}
