using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Immigration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Chamedoon.ApplicationTests.Immigration;

public class ImmigrationAiQueryHandlerIntegrationTests
{
    [Fact]
    public async Task Handle_Integration_CallsGroqAndReturnsFiveCountries()
    {
        var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");
        var model = Environment.GetEnvironmentVariable("GROQ_MODEL");
        var baseUrl = Environment.GetEnvironmentVariable("GROQ_BASE_URL") ?? "https://api.groq.com/openai/v1/";

        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(model))
        {
            return;
        }

        var client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        var factory = new TestHttpClientFactory(client);
        var options = Options.Create(new GroqOptions
        {
            ApiKey = apiKey,
            BaseUrl = baseUrl,
            Model = model
        });

        var handler = new ImmigrationAiQueryHandler(factory, options, NullLogger<ImmigrationAiQueryHandler>.Instance);
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
}
