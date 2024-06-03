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
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("dotnetProject")
        .WithCloneTargetPath("../../repos")
        .WithProjectPath("src/Dutchskull.Aspire.Git.Web/Dutchskull.Aspire.Git.Web.csproj"))
    .AddProject()
    .WithReference(cache)
    .WithReference(apiService);

var npmGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("npmProject")
        .WithCloneTargetPath("../../repos")
        .WithProjectPath("src/Dutchskull.Aspire.Git.React"))
    .AddNpmApp()
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

var nodeGitRepo = builder
    .AddGitRepository(c => c
        .WithGitUrl("https://github.com/Dutchskull/Aspire-Git.git")
        .WithName("nodeProject")
        .WithCloneTargetPath("../../repos")
        .WithProjectPath("src/Dutchskull.Aspire.Git.Node"))
    .AddNpmApp(scriptName: "watch")
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