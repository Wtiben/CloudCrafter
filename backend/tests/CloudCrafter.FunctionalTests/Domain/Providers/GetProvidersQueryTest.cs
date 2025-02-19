using CloudCrafter.Core.Commands.Providers.Github;
using CloudCrafter.Domain.Entities;
using CloudCrafter.Infrastructure.Data.Fakeds;
using FluentAssertions;
using NUnit.Framework;

namespace CloudCrafter.FunctionalTests.Domain.Providers;

using static Testing;

public class GetProvidersQueryTest : BaseTestFixture
{
    [Test]
    public void ShouldThrowExceptionWhenUserIsNotLoggedIn()
    {
        Assert.ThrowsAsync<UnauthorizedAccessException>(
            async () => await SendAsync(new GetProvidersQuery.Query())
        );
    }

    [Test]
    public async Task ShouldBeAbleToFetchProvidersList()
    {
        await RunAsAdministratorAsync();
        (await CountAsync<GithubProvider>()).Should().Be(0);

        var providers = FakerInstances.GithubProviderFaker.Generate(10);
        foreach (var provider in providers)
        {
            await AddAsync(provider);
        }

        var result = await SendAsync(new GetProvidersQuery.Query());
        result.Github.Count().Should().Be(10);

        foreach (var provider in result.Github)
        {
            provider.Name.Should().NotBeNullOrEmpty();
            provider.Id.Should().NotBe(Guid.Empty);
        }
    }
}
