using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;
using System.Net;
using System.Threading;
using Xunit.Abstractions;

namespace Dutchskull.Aspire.PolyRepo.Tests.E2E;

public class ServiceDiscoveryTests : IAsyncLifetime
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

    private DistributedApplication _distributedApplication = default!;

    [Theory]
    [InlineData("reactProject", "/", HttpStatusCode.NotModified)]
    [InlineData("nodeProject", "/", HttpStatusCode.OK)]
    [InlineData("dotnetProject", "/", HttpStatusCode.OK)]
    [InlineData("apiservice", "/weatherforecast", HttpStatusCode.OK)]
    public async Task AppHost_WhenStarted_ExpectServiceToExist(string project, string path, HttpStatusCode code)
    {
        // Act
        HttpClient httpClient = _distributedApplication.CreateHttpClient(project);

        await _distributedApplication.ResourceNotifications.WaitForResourceHealthyAsync(
             project)
             .WaitAsync(DefaultTimeout);

        HttpResponseMessage response = await httpClient.GetAsync(path);

        // Assert
        response.StatusCode.Should().Be(code);
    }

    public async Task DisposeAsync()
    {
        await _distributedApplication.StopAsync();
        await _distributedApplication.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        CancellationToken cancellationToken = CancellationToken.None;

        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Dutchskull_Aspire_PolyRepo_AppHost>(cancellationToken);

        appHost.Services.AddLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Debug);
            logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
            logging.AddFilter("Aspire.", LogLevel.Debug);
        });

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _distributedApplication = await appHost.BuildAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);

        await _distributedApplication.StartAsync(cancellationToken).WaitAsync(DefaultTimeout, cancellationToken);
    }
}