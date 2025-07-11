using Recipify.Data;
using Recipify.Data.Models;
using Recipify.Services.Core.Contracts;
using Recipify.Web.ViewModels.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipify.Services.Core
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext dbContext;

        public CommentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddCommentAsync(CreateCommentInputModel input, string userId)
        {
            var comment = new Comment
            {
                Content = input.Content,
                RecipeId = input.RecipeId,
               
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }
    }
}
