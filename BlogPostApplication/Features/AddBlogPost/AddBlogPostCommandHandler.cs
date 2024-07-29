using BlogPostApplication.Database;
using BlogPostApplication.Domain;
using MediatR;

namespace BlogPostApplication.Features.AddBlogPost
{
    public class AddBlogPostCommandHandler(BlogPostDbContext dbContext) : IRequestHandler<AddBlogPostCommand, int>
    {
        public async Task<int> Handle(AddBlogPostCommand request, CancellationToken cancellationToken)
        {
            var post = new BlogPost 
            {
                Title = request.Title,
                Content = request.Content 
            };

            dbContext.Add(post);
            await dbContext.SaveChangesAsync(cancellationToken);

            return post.Id;
        }
    }
}
