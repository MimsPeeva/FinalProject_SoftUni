using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Web.ViewModels.Recipe
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; } = null!;

    }
}
