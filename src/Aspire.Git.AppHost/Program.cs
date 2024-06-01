using Aspire.Git;

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Projects.Aspire_Git_ApiService>("apiservice");

IResourceBuilder<ProjectResource> webService = builder
    .AddProject<Projects.Aspire_Git_Web>("webservice")
    .WithReference(apiService);

var dotnetGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("dotnetProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.Web/Aspire.Git.Web.csproj"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddProject();

var npmGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("npmProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.React"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddNodeApp();

var nodeGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("nodeProject")
        .WithRepositoryPath("../../repos")
        .WithRelativeProjectPath("src/Aspire.Git.Node"))
    .WithReference(cache)
    .WithReference(apiService)
    .AddNpmApp();

builder
    .Build()
    .Run();