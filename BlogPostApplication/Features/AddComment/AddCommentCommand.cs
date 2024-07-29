using MediatR;

namespace BlogPostApplication.Features.AddComment
{
    public class AddCommentCommand : IRequest
    {
        public int BlogPostId { get; set; }
        public required AddCommentModel Model { get; set; }
    }
}
