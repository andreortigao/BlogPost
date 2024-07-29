using MediatR;

namespace BlogPostApplication.Features.AddBlogPost
{
    public class AddBlogPostCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
