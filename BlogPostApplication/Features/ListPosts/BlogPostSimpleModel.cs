namespace BlogPostApplication.Features.ListPosts
{
    public class BlogPostSimpleModel
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required int CommentCount {  get; set; }
    }
}