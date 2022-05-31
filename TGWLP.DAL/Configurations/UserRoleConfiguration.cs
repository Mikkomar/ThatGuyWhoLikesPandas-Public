using TGWLP.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");
            builder.HasKey(x => new { x.UserId, x.RoleId });
            builder.Property(x => x.UserId).HasColumnName("UserId");
            builder.Property(x => x.RoleId).HasColumnName("RoleId");
            builder.Property(x => x.BeginDate).HasColumnName("BeginDate");
            builder.Property(x => x.EndDate).HasColumnName("EndDate");
            builder.Property(x => x.Created).HasColumnName("Created");
            builder.Property(x => x.Creator).HasColumnName("Creator");
            builder.Property(x => x.Editor).HasColumnName("Editor");
            builder.Property(x => x.Edited).HasColumnName("Edited");

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(r => r.UserId);

            builder.HasOne(x => x.Role)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(r => r.RoleId);
        }
    }
}
