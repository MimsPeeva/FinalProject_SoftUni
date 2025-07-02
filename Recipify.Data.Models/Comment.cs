using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
       // public int UserId { get; set; }
      //  public User User { get; set; }
    }
}
