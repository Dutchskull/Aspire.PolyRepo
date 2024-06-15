using System.Net;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Dutchskull.Aspire.PolyRepo.Tests.E2E;

public class ServiceDiscoveryTests : IAsyncLifetime
{
    private DistributedApplication _distributedApplication = default!;

    [Theory]
    [InlineData("reactProject", "/")]
    [InlineData("nodeProject", "/")]
    [InlineData("dotnetProject", "/")]
    [InlineData("apiservice", "/weatherforecast")]
    public async Task AppHost_WhenStarted_ExpectServiceToExist(string project, string path)
    {
        // Act
        var httpClient = _distributedApplication.CreateHttpClient(project);
        var response = await httpClient.GetAsync(path);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public async Task DisposeAsync()
    {
        await _distributedApplication.StopAsync();
        await _distributedApplication.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Dutchskull_Aspire_PolyRepo_AppHost>();

        _distributedApplication = await appHost.BuildAsync();
        await _distributedApplication.StartAsync();
    }
}