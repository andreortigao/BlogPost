using BlogPostApplication.Database;
using BlogPostApplication.Domain;
using BlogPostApplication.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApplication.Features.GetPost
{
    public class GetBlogPostQueryHandler(BlogPostDbContext dbContext) : IRequestHandler<GetBlogPostQuery, BlogPostDetailModel>
    {
        public async Task<BlogPostDetailModel> Handle(GetBlogPostQuery request, CancellationToken cancellationToken)
        {
            var blogPost = await dbContext.BlogPosts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if(blogPost == null)
            {
                throw new NotFoundException(nameof(BlogPost), request.Id);
            }

            return new BlogPostDetailModel
            {
                Title = blogPost.Title,
                Content = blogPost.Content,
                Comments = blogPost.Comments.Select(x => new BlogPostCommentModel
                {
                    UserName =  x.UserName,
                    Content = x.Content
                }).ToList(),
            };
        }
    }
}
