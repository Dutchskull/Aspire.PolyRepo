using Dutchskull.Aspire.Git;
using Microsoft.Extensions.Hosting;

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Projects.Dutchskull_Aspire_Git_ApiService>("apiservice")
    .WithReference(cache)
    .WithExternalHttpEndpoints();

var dotnetGitRepo = builder
    .AddProjectGitRepository(c => c
            .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
            .WithCloneTargetPath("../../repos")
            .WithProjectPath("src/Dutchskull.Aspire.Git.Web/Dutchskull.Aspire.Git.Web.csproj"),
        name: "dotnetProject")
    .WithReference(cache)
    .WithReference(apiService);

var npmGitRepo = builder
    .AddNpmGitRepository(c => c
            .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
            .WithCloneTargetPath("../../repos")
            .WithProjectPath("src/Dutchskull.Aspire.Git.React"),
        name: "reactProject")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

var nodeGitRepo = builder
    .AddNpmGitRepository(c => c
            .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
            .WithCloneTargetPath("../../repos")
            .WithProjectPath("src/Dutchskull.Aspire.Git.Node"),
        scriptName: "watch",
        name: "nodeProject")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(54622);

if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    npmGitRepo.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
    nodeGitRepo.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder
    .Build()
    .Run();