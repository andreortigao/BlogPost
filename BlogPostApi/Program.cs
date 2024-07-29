using BlogPostApplication;
using BlogPostApplication.Database;
using BlogPostApplication.Features.AddBlogPost;
using BlogPostApplication.Features.GetPost;
using BlogPostApplication.Features.ListPosts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BlogPostApplication.Behaviors;
using BlogPostApi;
using BlogPostApplication.Features.AddComment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Application.Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(Application.Assembly);
builder.Services.AddDbContext<BlogPostDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapGet("/api/posts", async ([FromServices]ISender sender) =>
{
    return await sender.Send(new ListBlogPostsQuery());
})
.WithName("ListPosts")
.WithOpenApi()
.WithDescription("Get a list of posts");


app.MapGet("/api/posts/{id}", async (int id, [FromServices]ISender sender, CancellationToken cancellationToken) =>
{
    return await sender.Send(new GetBlogPostQuery { Id = id }, cancellationToken);
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

app.MapPost("/api/posts/{id}/comments", async (int id, [FromBody]AddCommentModel model, [FromServices]ISender sender, CancellationToken cancellationToken) =>
{
    var command = new AddCommentCommand { Model = model, BlogPostId = id };
    await sender.Send(command, cancellationToken);
    return Results.Created();
})
.WithName("AddComment")
.WithOpenApi()
.WithDescription("Add a new comment to a post");

app.Run();

public partial class Program { }