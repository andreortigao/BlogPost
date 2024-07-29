using BlogPostApplication.Database;
using BlogPostApplication.Domain;
using BlogPostApplication.Features.AddBlogPost;
using BlogPostApplication.Features.GetPost;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BlogPostApi.IntegrationTests
{
    public class BlogPostTests : IClassFixture<BlogPostApplicationFactory<Program>>
    {
        const string _url = "api/posts";
        private readonly BlogPostApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public BlogPostTests(BlogPostApplicationFactory<Program> factory)
        {
            _factory = factory;
             _client = factory.CreateClient();
        }

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
            var commentFaker = new Faker<Comment>()
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.Content, f => f.Rant.Review());

            var blogPostFaker = new Faker<BlogPost>()
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.Content, f => f.Lorem.Text())
                .RuleFor(x => x.Comments, commentFaker.Generate(5));

            var blogPost = blogPostFaker.Generate();

            using(var scope = _factory.Services.CreateScope())
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
    }
}
