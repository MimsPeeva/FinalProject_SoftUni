using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class DeleteRecipeInputModel
    {
        public int Id { get; set; }

        public string? Title { get; set; } 

        public string? ShortDescription { get; set; } 
        public string? Instructions { get; set; }  

        public string? ImageUrl { get; set; }

        public string? Category { get; set; } 
        public string? Cuisine { get; set; } 
        public string? DifficultyLevel { get; set; } 
    }
}
