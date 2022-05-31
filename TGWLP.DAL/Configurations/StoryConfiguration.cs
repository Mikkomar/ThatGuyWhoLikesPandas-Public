using TGWLP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TGWLP.DAL.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder) {
            builder.ToTable("Story");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasColumnName("Title");
            builder.Property(x => x.Text).HasColumnName("Text");
            builder.Property(x => x.BookId).HasColumnName("BookId");
            builder.Property(x => x.PublishDate).HasColumnName("PublishDate");
            builder.Property(x => x.SaveDate).HasColumnName("SaveDate");
            builder.Property(x => x.Created).HasColumnName("Created");
            builder.Property(x => x.Creator).HasColumnName("Creator");
            builder.Property(x => x.Edited).HasColumnName("Edited");
            builder.Property(x => x.Editor).HasColumnName("Editor");
        }
    }
}
