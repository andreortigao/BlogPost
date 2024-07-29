using BlogPostApplication.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApplication.Features.ListPosts
{
    public class ListBlogPostsQueryHandler(BlogPostDbContext dbContext) : IRequestHandler<ListBlogPostsQuery, IEnumerable<BlogPostSimpleModel>>
    {
        public async Task<IEnumerable<BlogPostSimpleModel>> Handle(ListBlogPostsQuery request, CancellationToken cancellationToken)
        {
            var blogPosts = await dbContext.BlogPosts
                .Select(x => new { x.Id, x.Title })
                .ToListAsync(cancellationToken);

            var commentCount = await dbContext.Comments
                .GroupBy(p => p.BlogPostId)
                .Select(g => new { BlogPostId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.BlogPostId, x => x.Count, cancellationToken);

            var models = blogPosts.Select(x => new BlogPostSimpleModel
            {
                Id = x.Id,
                Title = x.Title,
                CommentCount = commentCount.GetValueOrDefault(x.Id)
            });

            return models;
        }
    }
}
