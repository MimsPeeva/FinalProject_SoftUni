﻿using Microsoft.EntityFrameworkCore;
using Recipify.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Recipify.GCommon.ValidationConstants.DifficultyLevel;
namespace Recipify.Data.Configuration
{
    public class DifficultyLevelConfiguration : IEntityTypeConfiguration<DifficultyLevel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DifficultyLevel> builder)
        {
          builder.HasKey(dl => dl.Id);

            builder.Property(dl => dl.Level)
                .IsRequired()
                .HasMaxLength(DifficultyLevelMaxLength);

            builder.HasData(this.GenerateSeedDifficultyLevels());

        }
        private List<DifficultyLevel> GenerateSeedDifficultyLevels()
        {
            return new List<DifficultyLevel>
            {
                new DifficultyLevel
                {
                    Id = 1,
                    Level = "Easy"
                },
                new DifficultyLevel
                {
                    Id = 2,
                    Level = "Medium"
                },
                new DifficultyLevel
                {
                    Id = 3,
                    Level = "Hard"
                }
            };
        }
    }
}
