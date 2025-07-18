using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }=null!;
        public string Description { get; set; } = null!;
        public string Instructions { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public virtual ICollection<Ingredient> Ingredients { get; set; }
            = new List<Ingredient>();
        public List<Comment> Comments { get; set; }
        public int DifficultyId { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public int CuisineId { get; set; }
        public Cuisine Cuisine { get; set; }
    }
}
