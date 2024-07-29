using BlogPostApplication;
using BlogPostApplication.Database;
using BlogPostApplication.Features.AddBlogPost;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.Assembly));
builder.Services.AddDbContext<BlogPostDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/posts", () =>
{
    throw new NotImplementedException();
})
.WithName("ListPosts")
.WithOpenApi()
.WithDescription("Get a list of posts");


app.MapGet("/api/posts/{id}", (int id) =>
{
    throw new NotImplementedException();
})
.WithName("GetPost")
.WithOpenApi()
.WithDescription("Get a single post with their comments");

app.MapPost("/api/posts/", async ([FromBody]AddBlogPostCommand command, [FromServices]ISender sender, CancellationToken cancellationToken) =>
{
    var id = await sender.Send(command, cancellationToken);
    return Results.CreatedAtRoute("GetPost", new { id });
})
.WithName("CreatePost")
.WithOpenApi()
.WithDescription("Create a new post");

app.MapPost("/api/posts/{id}/comments", (int id) =>
{
    throw new NotImplementedException();
})
.WithName("AddComment")
.WithOpenApi()
.WithDescription("Add a new comment to a post");

app.Run();

public partial class Program { }