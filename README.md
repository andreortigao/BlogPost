# BlogPost API
This project is a simple Blog API built with ASP.NET Core and Entity Framework Core, as a code challenge.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) for compiling and running the project.
- [Docker](https://www.docker.com/get-started) for running integrated tests
- [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for data storage. Alternatively, you can use a docker image, e.g. `mcr.microsoft.com/mssql/server:2022-latest`
- [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) for running the database migrations.

## Setup for running the project locally:

### Clone the Repository

```bash
git clone git@github.com:andreortigao/BlogPost.git
cd BlogPost
```

### Configure default connection string using user-secret

```
cd BlogPostApi
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<YOUR CONNECTION STRING HERE>"
```

### Apply database migrations

```
cd BlogPostApi
dotnet ef database update
```

### Run the project 

```
cd BlogPostApi
dotnet run
```

## Next Steps

If I had more time available, I'd add open telemetry / improve logging. 

If necessary, I could also improve GetBlogPosts performance by making it in a single database roundtrip.