using BlogPostApplication.Database;
using BlogPostApplication.Domain;
using BlogPostApplication.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApplication.Features.AddComment
{
    public class AddCommentCommandHandler(BlogPostDbContext dbContext) : IRequestHandler<AddCommentCommand>
    {
        public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var blogPost = await dbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == request.BlogPostId, cancellationToken);

            if(blogPost is null)
            {
                throw new NotFoundException(nameof(BlogPost), request.BlogPostId);
            }

            var comment = new Comment
            { 
                UserName = request.Model.UserName,
                Content = request.Model.Content,
                BlogPost = blogPost
            };

            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
