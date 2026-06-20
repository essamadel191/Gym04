using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.FluentConfigurations
{
    public class MemeberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(p => p.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(m => m.HealthRecord)
                   .WithOne(h => h.Member)
                   .HasForeignKey<HealthRecord>(m => m.MemberId);

            // Required to implement the GymUserConfigurations
            base.Configure(builder);
        }
    }
}
