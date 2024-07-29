namespace BlogPostApplication.Domain
{
    public class BlogPost
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public List<Comment> Comments { get; set; } = [];
    }
}
