using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recipify.Data.Models;
using Recipify.GCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.User;
namespace Recipify.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(UsernameMaxLength);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(EmailMaxLength);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasData(this.GenerateSeedUsers());

        }
        private List<User> GenerateSeedUsers()
        {
            List<User> seedUsers = new List<User>()

            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin123.gmail.com"
                },
                new User
                {
                    Id = 2,
                    Username = "adi23",
                    Email = "adi23@mail.com"
                },
                new User
                {
                    Id = 3,
                    Username = "gosho45",
                    Email = "gosho1gosho@gmail.com"
                },

            };
            return seedUsers;

        }
    }
}