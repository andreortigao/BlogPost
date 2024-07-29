
namespace BlogPostApplication.Features.GetPost
{
    public class BlogPostDetailModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required List<BlogPostCommentModel> Comments { get; set; }
    }
}