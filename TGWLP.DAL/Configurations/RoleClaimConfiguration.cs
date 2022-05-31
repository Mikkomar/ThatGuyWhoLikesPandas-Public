using TGWLP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TGWLP.DAL.Configurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaim");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.ClaimType).HasColumnName("ClaimType");
            builder.Property(x => x.ClaimValue).HasColumnName("ClaimValue");
            builder.Property(x => x.RoleId).HasColumnName("RoleId");

            builder.HasOne(x => x.Role)
                .WithMany(t => t.Claims)
                .HasForeignKey(n => n.RoleId);
        }
    }
}
