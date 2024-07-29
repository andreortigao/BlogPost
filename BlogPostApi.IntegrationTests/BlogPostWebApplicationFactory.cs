using BlogPostApplication.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace BlogPostApi.IntegrationTests
{
    public class BlogPostApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
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

                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BlogPostDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var connectionString = _msSqlContainer.GetConnectionString();

                services.AddDbContext<BlogPostDbContext>(options =>
                {
                    options.UseSqlServer(connectionString);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<BlogPostDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }

        public override async ValueTask DisposeAsync()
        {
            await _msSqlContainer.DisposeAsync();
            await base.DisposeAsync();
        }

        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices(services =>
        //    {
        //        var descriptor = services.SingleOrDefault(
        //            d => d.ServiceType == typeof(DbContextOptions<BlogPostDbContext>));

        //        if (descriptor != null)
        //        {
        //            services.Remove(descriptor);
        //        }

        //        var connectionString = new SqlConnectionStringBuilder
        //        {
        //            DataSource = "localhost,1434",
        //            UserID = "sa",
        //            Password = "Your_password123",
        //            InitialCatalog = "TestDatabase"
        //        }.ConnectionString;

        //        services.AddDbContext<BlogContext>(options =>
        //        {
        //            options.UseSqlServer(connectionString);
        //        });

        //        var sp = services.BuildServiceProvider();

        //        using (var scope = sp.CreateScope())
        //        {
        //            var scopedServices = scope.ServiceProvider;
        //            var db = scopedServices.GetRequiredService<BlogContext>();
        //            db.Database.EnsureDeleted();
        //            db.Database.EnsureCreated();
        //        }
        //    });
        //}
    }
}
