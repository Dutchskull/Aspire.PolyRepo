using Aspire.Git;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder.AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Aspire_Git_ApiService>("apiservice");

//builder.AddProject<Aspire_Git_Web>("webfrontend")
//    .WithReference(cache)
//    .WithReference(apiService);

builder.AddGitProject(
    "https://github.com/Dutchskull/Aspire-Git.git",
    repositoryPath: "../../",
    relativeProjectPath: "src/Aspire.Git.Web/Aspire.Git.Web.csproj")
        .WithReference(cache)
        .WithReference(apiService);

builder.Build().Run();