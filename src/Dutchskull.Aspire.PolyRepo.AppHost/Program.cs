using Aspire.Hosting.Lifecycle;
using Dutchskull.Aspire.PolyRepo;
using Dutchskull.Aspire.PolyRepo.AppHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder
    .AddRedis("cache");

IResourceBuilder<ProjectResource> apiService = builder
    .AddProject<Dutchskull_Aspire_PolyRepo_ApiService>("apiservice")
    .WithReference(cache)
    .WithExternalHttpEndpoints();

IResourceBuilder<RepositoryResource> repository = builder.AddRepository(
    "repository",
    "https://github.com/Dutchskull/Aspire-Git.git",
    c => c
        .WithDefaultBranch("develop")
        .KeepUpToDate()
        .WithTargetPath("../../repos"));

IResourceBuilder<ProjectResource> dotnetProject = builder
    .AddProjectFromRepository("dotnetProject", repository,
        "src/Dutchskull.Aspire.PolyRepo.Web/Dutchskull.Aspire.PolyRepo.Web.csproj")
    .WithReference(cache)
    .WithReference(apiService);


IResourceBuilder<NodeAppResource> reactProject = builder
    .AddNpmAppFromRepository("reactProject", repository, "src/Dutchskull.Aspire.PolyRepo.React")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(3000);

IResourceBuilder<NodeAppResource> nodeProject = builder
    .AddNodeAppFromRepository("nodeProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithReference(apiService)
    .WithHttpEndpoint(54622);

builder.Services.TryAddEnumerable(ServiceDescriptor
    .Singleton<IDistributedApplicationLifecycleHook, NodeAppAddPortLifecycleHook>());

if (builder.Environment.IsDevelopment() &&
    builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    reactProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
    nodeProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder
    .Build()
    .Run();