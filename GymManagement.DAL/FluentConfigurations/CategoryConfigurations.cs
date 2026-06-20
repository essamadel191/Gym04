using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.FluentConfigurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.CategoryName)
                .HasColumnType("varchar")
                .HasMaxLength(30);
            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");


            //Seeding Categories
            builder.HasData(
                new Category {  Id = 1, CategoryName = "Cardio" },
                new Category {  Id = 2, CategoryName = "Strength" },
                new Category { Id = 3, CategoryName = "Yoga" },
                new Category { Id = 4, CategoryName = "Booking" },
                new Category { Id = 5, CategoryName = "CrossFit" }
            );
        }
    }
}
