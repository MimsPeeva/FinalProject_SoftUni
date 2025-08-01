﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Quantity { get; set; } = null!;
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; } 
    }
}
