using Microsoft.EntityFrameworkCore;
using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.Cuisine;
namespace Recipify.Data.Configuration
{
    public class CuisineConfiguration:IEntityTypeConfiguration<Cuisine>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cuisine> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(CuisineNameMaxLength);

            builder.HasData(this.GenerateSeedCuisines());

        }
        private List<Cuisine> GenerateSeedCuisines()
        {
            List<Cuisine> seedCuisines = new List<Cuisine>()
            {
                new Cuisine
                {
                    Id = 1,
                    Name = "Italian"
                },
                new Cuisine
                {
                    Id = 2,
                    Name = "Chinese"
                },
                new Cuisine
                {
                    Id = 3,
                    Name = "Indian"
                },
                new Cuisine
                {
                    Id = 4,
                    Name = "Mexican"
                },
                new Cuisine
                {
                    Id = 5,
                    Name = "French"
                },
                new Cuisine
                {
                    Id = 6,
                    Name = "Bulgarian"
                },
                new Cuisine
                {
                    Id = 7,
                    Name = "Turkish"
                },
                new Cuisine
                {
                    Id = 8,
                    Name = "Balkan"
                },
                new Cuisine
                {
                    Id = 9,
                    Name = "Greek"
                },

            };
            return seedCuisines;
        }

    }
}
