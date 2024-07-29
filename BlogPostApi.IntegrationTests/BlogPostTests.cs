using BlogPostApplication.Database;
using BlogPostApplication.Domain;
using BlogPostApplication.Features.AddBlogPost;
using BlogPostApplication.Features.AddComment;
using BlogPostApplication.Features.GetPost;
using BlogPostApplication.Features.ListPosts;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlogPostApi.IntegrationTests
{
    public class BlogPostTests(BlogPostApplicationFactory<Program> factory) : IClassFixture<BlogPostApplicationFactory<Program>>, IAsyncLifetime
    {
        const string _url = "api/posts";
        private readonly BlogPostApplicationFactory<Program> _factory = factory;
        private readonly HttpClient _client = factory.CreateClient();

        public static IEnumerable<object[]> AddBlogPostCommandGenerator()
        {
            var faker = new Faker<AddBlogPostCommand>()
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Content, f => f.Lorem.Text());

            return faker.Generate(5).Select(x => new object[] { x });
        }

        [Theory]
        [MemberData(nameof(AddBlogPostCommandGenerator))]
        public async Task CanCreateBlogPost(AddBlogPostCommand command)
        {
            var response = await _client.PostAsync(_url,
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CanGetBlogPost()
        {
            var blogPost = GeneratePost(5);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogPostDbContext>();

                dbContext.BlogPosts.Add(blogPost);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync($"{_url}/{blogPost.Id}");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var blogPostModel = JsonConvert.DeserializeObject<BlogPostDetailModel>(responseString);

            Assert.NotNull(blogPostModel);
            Assert.Equal(5, blogPostModel.Comments.Count);
        }

        [Fact]
        public async Task CanListPosts()
        {
            var posts = Enumerable.Range(5, 10).Select(i => GeneratePost(i)).ToList();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogPostDbContext>();

                dbContext.BlogPosts.AddRange(posts);
                await dbContext.SaveChangesAsync();
            }

            var response = await _client.GetAsync(_url);

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var blogPostModels = JsonConvert.DeserializeObject<List<BlogPostSimpleModel>>(responseString);
            Assert.NotNull(blogPostModels);
            Assert.Equal(10, blogPostModels.Count);
            Assert.Equal(Enumerable.Range(5, 10), blogPostModels.Select(x => x.CommentCount));
        }

        [Fact]
        public async Task CanCreateComment()
        {
            var blogPost = GeneratePost(5);

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogPostDbContext>();

                dbContext.BlogPosts.Add(blogPost);
                await dbContext.SaveChangesAsync();
            }
            var commentFake = _commentFaker.Generate();

            var command = new AddCommentModel
            {
                UserName = commentFake.UserName,
                Content = commentFake.Content
            };

            var response = await _client.PostAsync($"{_url}/{blogPost.Id}/comments",
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private static BlogPost GeneratePost(int? commentCount = null)
        {
            var blogPostFaker = new Faker<BlogPost>()
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Content, f => f.Lorem.Text())
                .RuleFor(x => x.Comments, f => _commentFaker.Generate(commentCount ?? f.Random.Number(1, 10)));

            return blogPostFaker.Generate();
        }

        private static readonly Faker<Comment> _commentFaker = new Faker<Comment>()
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.Content, f => f.Rant.Review());

        public async Task InitializeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlogPostDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<BlogPostDbContext>();
            await db.Database.EnsureDeletedAsync();
        }
    }
}
