﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class CategoriesDropdownModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
