using MediatR;

namespace BlogPostApplication.Features.ListPosts
{
    public class ListBlogPostsQuery : IRequest<IEnumerable<BlogPostSimpleModel>>
    {
    }
}
