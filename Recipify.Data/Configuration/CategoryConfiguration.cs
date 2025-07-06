using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Category;

namespace Recipify.Data.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(CategoryNameMaxLength);

            builder.HasData(this.GenerateSeedCategories());

        }
        private List<Category> GenerateSeedCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Breakfast"
                },
                new Category
                {
                    Id = 2,
                    Name = "Brunch"
                },
                new Category
                {
                    Id = 3,
                    Name = "Lunch"
                },
                new Category
                {
                    Id = 4,
                    Name = "Dinner"
                },
                new Category
                {
                    Id = 5,
                    Name = "Dessert"
                },
                 new Category
                {
                    Id = 6,
                    Name = "Salads"
                },
                  new Category
                {
                    Id = 7,
                    Name = "Soups"
                }
            };
        }
    }
}
