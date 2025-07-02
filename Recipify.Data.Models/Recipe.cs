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
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Ingredient> Ingredients { get; set; }
            //= new HashSet<Ingredient>();
       public List<Comment> Coments { get; set; }
    }
}
