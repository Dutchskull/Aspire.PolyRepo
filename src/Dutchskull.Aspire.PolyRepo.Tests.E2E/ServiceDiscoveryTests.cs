using System.Net;
using FluentAssertions;
using Projects;

namespace Dutchskull.Aspire.PolyRepo.Tests.E2E;

public class ServiceDiscoveryTests : IAsyncLifetime
{
    private DistributedApplication _distributedApplication = default!;

    public async Task DisposeAsync()
    {
        Directory.Delete("../../repos", true);
        await _distributedApplication.StopAsync();
        await _distributedApplication.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        IDistributedApplicationTestingBuilder appHost =
            await DistributedApplicationTestingBuilder.CreateAsync<Dutchskull_Aspire_PolyRepo_AppHost>();

        _distributedApplication = await appHost.BuildAsync();
        await _distributedApplication.StartAsync();
    }

    [Theory]
    [InlineData("reactProject", "/", HttpStatusCode.NotModified)]
    [InlineData("nodeProject", "/", HttpStatusCode.OK)]
    [InlineData("dotnetProject", "/", HttpStatusCode.OK)]
    [InlineData("apiservice", "/weatherforecast", HttpStatusCode.OK)]
    public async Task AppHost_WhenStarted_ExpectServiceToExist(string project, string path, HttpStatusCode code)
    {
        // Act
        HttpClient httpClient = _distributedApplication.CreateHttpClient(project);
        // httpClient.Timeout = TimeSpan.FromSeconds(30);
        HttpResponseMessage response = await httpClient.GetAsync(path);

        // Assert
        response.StatusCode.Should().Be(code);
    }
}