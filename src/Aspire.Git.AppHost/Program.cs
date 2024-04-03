using Aspire.Git;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder.AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Aspire_Git_ApiService>("apiservice");

IResourceBuilder<GitRepositoryResource> dotnetGitRepo = builder.AddGitRepository(
    "https://github.com/Dutchskull/Aspire-Git.git",
    name: "dotnetProject",
    repositoryPath: "../../",
    relativeProjectPath: "src/Aspire.Git.Web/Aspire.Git.Web.csproj")
        .WithReference(cache)
        .WithReference(apiService);

IResourceBuilder<GitRepositoryResource> npmGitRepo = builder.AddGitRepository(
    "https://github.com/Dutchskull/Aspire-Git.git",
    name: "npmProject",
    repositoryPath: "../../",
    relativeProjectPath: "src/Aspire.Git.React")
        .WithReference(cache)
        .WithReference(apiService);

IResourceBuilder<GitRepositoryResource> nodeGitRepo = builder.AddGitRepository(
    "https://github.com/Dutchskull/Aspire-Git.git",
    name: "nodeProject",
    repositoryPath: "../../",
    relativeProjectPath: "src/Aspire.Git.Node")
        .WithReference(cache)
        .WithReference(apiService);

dotnetGitRepo.AddProject();

nodeGitRepo.AddNodeApp();

npmGitRepo.AddNpmApp();

builder.Build().Run();