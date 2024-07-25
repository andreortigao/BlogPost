var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapPost("/api/posts/", () =>
{
    throw new NotImplementedException();
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
