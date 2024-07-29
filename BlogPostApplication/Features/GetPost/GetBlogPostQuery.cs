using MediatR;

namespace BlogPostApplication.Features.GetPost
{
    public class GetBlogPostQuery : IRequest<BlogPostDetailModel>
    {
        public int Id { get; set; }
    }
}
