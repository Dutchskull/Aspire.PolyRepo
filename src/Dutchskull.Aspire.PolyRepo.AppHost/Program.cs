using Aspire.Hosting;
using Aspire.Hosting.Lifecycle;
using Dutchskull.Aspire.PolyRepo;
using Dutchskull.Aspire.PolyRepo.AppHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Projects.Dutchskull_Aspire_PolyRepo_ApiService>("apiservice")
    .WithReference(cache)
    .WithExternalHttpEndpoints();

var repository = builder.AddRepository(
    "repository",
    "https://github.com/Dutchskull/Aspire-Git.git",
    c => c.WithDefaultBranch("feature/refactor")
        .WithTargetPath("../../repos"));

var dotnetProject = builder
    .AddProjectFromRepository("dotnetProject", repository, "src/Dutchskull.Aspire.PolyRepo.Web/Dutchskull.Aspire.PolyRepo.Web.csproj")
    .WithReference(cache)
    .WithReference(apiService);

var reactProject = builder
    .AddNpmAppFromRepository("reactProject", repository, "src/Dutchskull.Aspire.PolyRepo.React")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

var nodeProject = builder
    .AddNodeAppFromRepository("nodeProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(54622);

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

if (builder.Environment.IsDevelopment() &&
    builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    reactProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
    nodeProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder
    .Build()
    .Run();