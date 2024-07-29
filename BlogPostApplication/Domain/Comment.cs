namespace BlogPostApplication.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Content { get; set; }
        public required virtual BlogPost BlogPost { get; set; } = null!;
        public int BlogPostId {  get; set; }

    }
}
