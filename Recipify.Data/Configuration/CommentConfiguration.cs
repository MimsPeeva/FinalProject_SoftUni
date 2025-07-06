using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Comment;
namespace Recipify.Data.Configuration
{
    public class CommentConfiguration:IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(CommentContentMaxLength);

            builder.Property(c => c.Author)
                .IsRequired()
                .HasMaxLength(CommentAuthorMaxLength);

            builder.HasOne(c => c.Recipe)
                .WithMany(r => r.Coments)
                .HasForeignKey(c => c.RecipeId);

            builder.HasData(this.GenerateSeedComments());

        }
        private List<Comment> GenerateSeedComments()
        {
            return new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Content = "This recipe is amazing!",
                    Author = "John Soy",
                    RecipeId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "I love this dish!",
                    Author = "Jane Smith",
                    RecipeId = 1
                },
                new Comment
                {
                    Id = 3,
                    Content = "Not my favorite, but still good.",
                    Author = "Alice Johnson",
                    RecipeId = 2
                },
                 new Comment
                {
                    Id = 4,
                    Content = "Good recipe. You can add move vegetables for better taste.",
                    Author = "Trey Looh",
                    RecipeId = 2
                },
                new Comment
                {
                    Id = 5,
                    Content = "So delisious! Me and my family loves this pancakes recipe!",
                    Author = "Kate Waing",
                    RecipeId = 3
                }
            };
        }

    }
}
