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

        public string? ImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;

        public string DifficultyLevel { get; set; } = null!;
        public string CuisineName { get; set; }  = null!;
        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}
