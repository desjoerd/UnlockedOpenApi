using System;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

using Unlocked.Client;
using Unlocked.Client.Models;

namespace Unlocked.Api.Tests.Endpoints;

public class GetLockByIdTest(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{

    private UnlockedApiClient CreateClient()
    {
        var customizedFactory = factory.WithWebHostBuilder(c =>
        {
            c.ConfigureTestServices(services =>
            {

            });
        });

        var httpClient = customizedFactory.CreateClient();

        var authProvider = new AnonymousAuthenticationProvider();
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);

        var unlockedApiClient = new UnlockedApiClient(adapter);
        return unlockedApiClient;
    }

    [Fact]
    public async Task GetLockById_Returns200Ok()
    {
        var client = CreateClient();
        var response = await client
            .Locks[Guid.Parse("7fbe9a8e-9961-4262-8160-831cf9e47e91")]
            .GetAsync();

        Assert.NotNull(response);
        Assert.Equal(LockStatus.Unlocked, response.LockStatus);
    }

    [Fact]
    public async Task GetLockById_Returns404NotFound()
    {
        var client = CreateClient();

        var problemDetails = await Assert.ThrowsAsync<ProblemDetails>(
            () => client
                .Locks[Guid.Empty]
                .GetAsync()
        );

        Assert.Equal(404, problemDetails.Status);
    }

}
