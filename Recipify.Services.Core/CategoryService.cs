﻿using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoriesDropdownModel>> GetAllCategoriesDropDownAsync()
        {
            //    return await this.dbContext
            //.Categories
            //.AsNoTracking()
            //.Select(c => new SelectListItem
            //{
            //    Value = c.Id.ToString(),
            //    Text = c.Name
            //})
            //.ToListAsync();

            //    return await dbContext
            //.Categories
            //.AsNoTracking()
            //.Select(c => new CategoriesDropdownModel
            //{
            //    Id = c.Id,
            //    Name = c.Name
            //})
            //.ToListAsync();

            IEnumerable<CategoriesDropdownModel> recipesAsDropDown = await this.dbContext
                  .Categories
                  .AsNoTracking()
                  .Select(t => new CategoriesDropdownModel()
                  {
                      Id = t.Id,
                      Name = t.Name
                  })
                  .ToArrayAsync();
            return recipesAsDropDown;
        }
    }
}
