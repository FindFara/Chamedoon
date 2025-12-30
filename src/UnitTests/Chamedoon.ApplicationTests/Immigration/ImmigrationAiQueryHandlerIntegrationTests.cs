using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Immigration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Chamedoon.ApplicationTests.Immigration;

public class ImmigrationAiQueryHandlerIntegrationTests
{
    [Fact]
    public async Task Handle_Integration_CallsGroqAndReturnsFiveCountries()
    {
        var options = LoadGroqOptions();
        if (options is null ||
            string.IsNullOrWhiteSpace(options.ApiKey) ||
            string.IsNullOrWhiteSpace(options.Model))
        {
            return;
        }

        var client = new HttpClient { BaseAddress = new Uri(options.BaseUrl) };
        var factory = new TestHttpClientFactory(client);
        var optionsWrapper = Options.Create(options);

        var handler = new ImmigrationAiQueryHandler(factory, optionsWrapper, NullLogger<ImmigrationAiQueryHandler>.Instance);
        var input = new ImmigrationInput
        {
            Age = 29,
            JobTitle = "توسعه‌دهنده نرم‌افزار",
            JobCategory = JobCategoryType.IT,
            WorkExperienceYears = 5,
            FieldCategory = FieldCategoryType.IT,
            DegreeLevel = DegreeLevelType.Bachelor,
            LanguageCertificate = LanguageCertificateType.IELTS,
            VisaType = VisaType.Work,
            HasCriminalRecord = false,
            InvestmentAmount = 30000,
            MaritalStatus = MaritalStatusType.Single,
            WillingToStudy = false
        };

        var result = await handler.Handle(new ImmigrationAiQuery { Input = input }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result.TopCountries.Count);
    }

    private sealed class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _client;

        public TestHttpClientFactory(HttpClient client)
        {
            _client = client;
        }

        public HttpClient CreateClient(string name) => _client;
    }

    private static GroqOptions? LoadGroqOptions()
    {
        var repoRoot = FindRepoRoot();
        if (repoRoot is null)
        {
            return null;
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(repoRoot)
            .AddJsonFile("src/Presentation/ChamedoonWebUI/appsettings.json", optional: true)
            .AddJsonFile("src/Presentation/ChamedoonWebUI/appsettings.Production.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        return configuration.GetSection(GroqOptions.SectionName).Get<GroqOptions>();
    }

    private static string? FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(
                directory.FullName,
                "src",
                "Presentation",
                "ChamedoonWebUI",
                "appsettings.json");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
