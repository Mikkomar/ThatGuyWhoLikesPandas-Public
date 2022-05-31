using TGWLP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id");
            builder.Property(x => x.FirstName).HasColumnName("FirstName");
            builder.Property(x => x.LastName).HasColumnName("LastName");
            builder.Property(x => x.Created).HasColumnName("Created");
            builder.Property(x => x.Creator).HasColumnName("Creator");
            builder.Property(x => x.Edited).HasColumnName("Edited");
            builder.Property(x => x.Editor).HasColumnName("Editor");
            
            builder.HasMany(x => x.UserRoles)
                .WithOne(m => m.User)
                .HasForeignKey(u => u.UserId);
        }
    }
}
