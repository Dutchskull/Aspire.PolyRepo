using FluentAssertions;
using Projects;

namespace Dutchskull.Aspire.PolyRepo.Tests.E2E;

public class ServiceDiscoveryTests : IAsyncLifetime
{
    private DistributedApplication _distributedApplication = default!;
    private ResourceNotificationService _resourceNotificationService;

    public async Task DisposeAsync()
    {
        await _distributedApplication.StopAsync();
        await _distributedApplication.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Dutchskull_Aspire_PolyRepo_AppHost>();

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _distributedApplication = await appHost.BuildAsync();

        _resourceNotificationService = _distributedApplication.Services
            .GetRequiredService<ResourceNotificationService>();

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

        await _resourceNotificationService.WaitForResourceAsync(
                project,
                KnownResourceStates.Running
            )
            .WaitAsync(TimeSpan.FromSeconds(30));

        HttpResponseMessage response = await httpClient.GetAsync(path);

        // Assert
        response.StatusCode.Should().Be(code);
    }
}