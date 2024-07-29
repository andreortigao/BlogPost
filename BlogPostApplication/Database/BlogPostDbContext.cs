using BlogPostApplication.Database.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApplication.Database
{
    public class BlogPostDbContext(DbContextOptions<BlogPostDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogPostMapping());
            modelBuilder.ApplyConfiguration(new CommentMapping());
        }
    }
}
