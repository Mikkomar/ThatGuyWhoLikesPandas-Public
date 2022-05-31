using TGWLP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Configurations 
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.NameEn).HasColumnName("NameEn");
            builder.Property(x => x.NameFi).HasColumnName("NameFi");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.Color).HasColumnName("Color");
            builder.Property(x => x.Created).HasColumnName("Created");
            builder.Property(x => x.Creator).HasColumnName("Creator");
            builder.Property(x => x.Edited).HasColumnName("Edited");
            builder.Property(x => x.Editor).HasColumnName("Editor");

            builder.HasMany(r => r.Claims)
                .WithOne(c => c.Role)
                .HasForeignKey(c => c.RoleId);
        }
    }
}
