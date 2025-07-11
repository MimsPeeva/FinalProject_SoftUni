using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class CreateCommentInputModel
    {
        public int RecipeId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

 
    }
}
