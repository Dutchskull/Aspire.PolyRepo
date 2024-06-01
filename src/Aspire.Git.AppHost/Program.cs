using Aspire.Git;

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Projects.Aspire_Git_ApiService>("apiservice");

var dotnetGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("dotnetProject")
        .WithRepositoryPath("../../")
        .WithRelativeProjectPath("src/Aspire.Git.Web/Aspire.Git.Web.csproj"))
    .WithReference(cache)
    .WithReference(apiService);

var npmGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("npmProject")
        .WithRepositoryPath("../../")
        .WithRelativeProjectPath("src/Aspire.Git.React"))
    .WithReference(cache)
    .WithReference(apiService);

var nodeGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("nodeProject")
        .WithRepositoryPath("../../")
        .WithRelativeProjectPath("src/Aspire.Git.Node"))
    .WithReference(cache)
    .WithReference(apiService);

dotnetGitRepo.AddProject();

npmGitRepo.AddNodeApp();

nodeGitRepo.AddNpmApp();

builder
    .Build()
    .Run();