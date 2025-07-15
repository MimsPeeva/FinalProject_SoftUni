using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class DetailsRecipeViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;
        public List<string> Ingredients { get; set; } = new List<string>();

        public string? ImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string Instructions { get; set; } = null!;

        public string DifficultyLevel { get; set; } = null!;
        public int DifficultyLevelId { get; set; }
        public int CuisineId { get; set; } 
        public string CuisineName { get; set; }  = null!;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
