using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GymManagement.DAL.Models;

namespace GymManagement.DAL.FluentConfigurations
{
    public class PlanConfigurations : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name)
                .HasColumnType("varchar")
                .HasMaxLength(30);

            builder.Property(p => p.Description)
                .HasMaxLength (200);

            builder.Property(p => p.Price)
                .HasPrecision (10,2);

            builder.Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.ToTable(TB =>
            {
                TB.HasCheckConstraint("PlanDurationChek", "DurationDays BETWEEN 1 AND 365");
            });
        }
    }
}
