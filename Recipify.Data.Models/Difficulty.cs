using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Models
{
    public class Difficulty
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public List<Recipe> Recipes { get; set; }
        // public ICollection<Recipe> Recipes { get; set; } = new HashSet<Recipe>();
    }
}
