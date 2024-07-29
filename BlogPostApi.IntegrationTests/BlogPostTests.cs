using BlogPostApplication.Features.AddBlogPost;
using Bogus;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace BlogPostApi.IntegrationTests
{
    public class BlogPostTests : IClassFixture<BlogPostApplicationFactory<Program>>
    {
        const string _url = "api/posts";
        private readonly HttpClient _client;

        public BlogPostTests(BlogPostApplicationFactory<Program> factory)
        {
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
    }
}
