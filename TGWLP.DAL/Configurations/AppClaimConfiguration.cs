using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TGWLP.DAL.Entities;

namespace TGWLP.DAL.Configurations
{
    public class AppClaimConfiguration : IEntityTypeConfiguration<AppClaim>
    {
        public void Configure(EntityTypeBuilder<AppClaim> builder)
        {
            builder.ToTable("Claim");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.ClaimType).HasColumnName("ClaimType");
            builder.Property(x => x.ClaimValue).HasColumnName("ClaimValue");
            builder.Property(x => x.NameEn).HasColumnName("NameEn");
            builder.Property(x => x.NameFi).HasColumnName("NameFi");
            builder.Property(x => x.DescriptionEn).HasColumnName("DescriptionEn");
            builder.Property(x => x.DescriptionFi).HasColumnName("DescriptionFi");
            builder.Property(x => x.Created).HasColumnName("Created");
            builder.Property(x => x.Creator).HasColumnName("Creator");
            builder.Property(x => x.Edited).HasColumnName("Edited");
            builder.Property(x => x.Editor).HasColumnName("Editor");
        }
    }
}
