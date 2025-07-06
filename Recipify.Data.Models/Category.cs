﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }= null!;
        public List<Recipe> Recipes { get; set; }
            //= new List<Recipe>();

    }
}
