using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class IngredientInputModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Quantity { get; set; } = null!;
    }
}
