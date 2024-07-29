using BlogPostApplication.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApplication.Database.Mappings
{
    internal class BlogPostMapping : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.HasKey(bp => bp.Id);

            builder.Property(bp => bp.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(bp => bp.Content)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.HasMany(bp => bp.Comments)
                .WithOne(c => c.BlogPost)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
