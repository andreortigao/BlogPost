using BlogPostApplication.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace BlogPostApi.IntegrationTests
{
    public sealed class BlogPostApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly MsSqlContainer _msSqlContainer;

        public BlogPostApplicationFactory()
        {
            _msSqlContainer = new MsSqlBuilder()
                //.WithPassword("Your_password123")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                await _msSqlContainer.StartAsync();

                var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BlogPostDbContext>));

                if (descriptor is not null && !services.IsReadOnly)
                {
                    services.Remove(descriptor);

                    var connectionString = _msSqlContainer.GetConnectionString();

                    services.AddDbContext<BlogPostDbContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                    });
                }
            });
        }

        public override async ValueTask DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await base.DisposeAsync();
        }
    }
}
