using Aspire.Hosting;
using Aspire.Hosting.JavaScript;
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
        .WithGitConfig(builder => builder.WithAuthentication("", ""))
        .WithTargetPath("../../repos"));

IResourceBuilder<ProjectResource> dotnetProject = builder
    .AddProjectFromRepository("dotnetProject", repository,
        "src/Dutchskull.Aspire.PolyRepo.Web/Dutchskull.Aspire.PolyRepo.Web.csproj")
    .WithReference(cache)
    .WithReference(apiService);

IResourceBuilder<JavaScriptAppResource> reactProject = builder
    .AddNpmAppFromRepository("reactProject", repository, "src/Dutchskull.Aspire.PolyRepo.React")
    .WithReference(cache)
    .WithReference(apiService)
    .WithNpm()
    .WithHttpEndpoint(3000);

IResourceBuilder<ViteAppResource> viteProject = builder
    .AddViteAppFromRepository("viteProject", repository, "src/Dutchskull.Aspire.PolyRepo.Vite")
    .WithReference(cache)
    .WithReference(apiService)
    .WithNpm()
    .WithHttpEndpoint(3001, name: "vite");

IResourceBuilder<NodeAppResource> nodeProject = builder
    .AddNodeAppFromRepository("nodeProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithReference(apiService)
    .WithNpm()
    .WithHttpEndpoint(54622);

IResourceBuilder<ContainerResource> dockerFile = builder
    .AddDockerFileFromRepository("dockerProject", repository, "src/Dutchskull.Aspire.PolyRepo.Node")
    .WithReference(cache)
    .WithEndpoint(scheme: "http", targetPort: 5555, env: "PORT")
    .WithBuildArg("GO_VERSION", "1.23rc1");

builder.Services.TryAddEnumerable(ServiceDescriptor
    .Singleton<IDistributedApplicationEventingSubscriber, JavascriptAppAddPortEventSubscriber>());

if (builder.Environment.IsDevelopment() &&
    builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    reactProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
    viteProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
    viteProject.WithEnvironment("VITE_ENVIRONMENT_MODE", "Development");
    nodeProject.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder
    .Build()
    .Run();