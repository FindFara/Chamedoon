using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Application.Services.Immigration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Chamedoon.ApplicationTests.Immigration;

public class ImmigrationAiQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidJson_ReturnsFiveCountries()
    {
        var response = CreateGroqResponse(BuildValidResultJson());
        var handler = CreateHandler(response);

        var result = await handler.Handle(new ImmigrationAiQuery { Input = new ImmigrationInput() }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(5, result.TopCountries.Count);
    }

    [Fact]
    public async Task Handle_InvalidJson_RepairsOnce_ThenFallsBack()
    {
        var responseOne = CreateGroqResponse("not json");
        var responseTwo = CreateGroqResponse("{\"TopCountries\":[]}");
        var handler = CreateHandler(responseOne, responseTwo);

        var result = await handler.Handle(new ImmigrationAiQuery { Input = new ImmigrationInput() }, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result.TopCountries);
        Assert.Equal(2, handler.CallCount);
    }

    private static TestImmigrationAiQueryHandler CreateHandler(params HttpResponseMessage[] responses)
    {
        var handler = new SequenceHandler(responses);
        var client = new HttpClient(handler) { BaseAddress = new System.Uri("https://api.groq.com/openai/v1/") };
        var factory = new TestHttpClientFactory(client);
        var options = Options.Create(new GroqOptions
        {
            ApiKey = "test",
            BaseUrl = "https://api.groq.com/openai/v1/",
            Model = "test-model"
        });

        return new TestImmigrationAiQueryHandler(factory, options, NullLogger<ImmigrationAiQueryHandler>.Instance, handler);
    }

    private static HttpResponseMessage CreateGroqResponse(string content)
    {
        var payload = JsonSerializer.Serialize(new
        {
            choices = new[]
            {
                new
                {
                    message = new
                    {
                        content
                    }
                }
            }
        });

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };
    }

    private static string BuildValidResultJson()
    {
        return """
        {
          "TopCountries": [
            {
              "Country": "Canada",
              "Score": 92,
              "RecommendedVisaType": "Skilled Worker",
              "Data": {
                "MaritalStatusImpact": "Single applicants usually have slightly more flexibility in points.",
                "IranianMigrationRestrictions": [
                  "Extra security screening may apply to Iranian nationals.",
                  "Longer processing times possible for background checks.",
                  "Some banks may request additional compliance documents."
                ],
                "InvestmentNotes": "General skilled pathways do not require large upfront investment.",
                "InvestmentAmount": 15000,
                "InvestmentCurrency": "CAD",
                "AdditionalInfo": "Strong demand for tech and healthcare roles across provinces.",
                "LivingCosts": {
                  "Housing": [],
                  "FamilyMonthly": "CAD 3200-4200",
                  "SingleMonthly": "CAD 2000-2800",
                  "Transport": "CAD 120-160 monthly pass",
                  "Utilities": "CAD 150-220",
                  "Recreation": "CAD 150-250",
                  "RentOneBedroom": "CAD 1400-2000",
                  "RentThreeBedroom": "CAD 2600-3400",
                  "Internet": "CAD 60-90"
                },
                "Job": null,
                "Education": null,
                "PersonalityReport": null,
                "LanguageRequirement": null
              }
            },
            {
              "Country": "Australia",
              "Score": 88,
              "RecommendedVisaType": "Skilled Independent",
              "Data": {
                "MaritalStatusImpact": "Partner skills can improve overall points if applicable.",
                "IranianMigrationRestrictions": [
                  "Background checks can extend processing times.",
                  "Additional identity verification may be requested.",
                  "Banking compliance checks may add documentation steps."
                ],
                "InvestmentNotes": "Skilled visas focus on occupation lists rather than investment.",
                "InvestmentAmount": 20000,
                "InvestmentCurrency": "AUD",
                "AdditionalInfo": "Regional sponsorship can improve eligibility.",
                "LivingCosts": {
                  "Housing": [],
                  "FamilyMonthly": "AUD 4200-5200",
                  "SingleMonthly": "AUD 2500-3200",
                  "Transport": "AUD 150-200 monthly pass",
                  "Utilities": "AUD 180-260",
                  "Recreation": "AUD 180-260",
                  "RentOneBedroom": "AUD 1600-2300",
                  "RentThreeBedroom": "AUD 2800-3600",
                  "Internet": "AUD 65-90"
                },
                "Job": null,
                "Education": null,
                "PersonalityReport": null,
                "LanguageRequirement": null
              }
            },
            {
              "Country": "Germany",
              "Score": 84,
              "RecommendedVisaType": "Blue Card",
              "Data": {
                "MaritalStatusImpact": "Family reunification is possible with sufficient income.",
                "IranianMigrationRestrictions": [
                  "Embassy appointment availability can affect timelines.",
                  "Document verification may require extra steps.",
                  "Some employers may request additional compliance checks."
                ],
                "InvestmentNotes": "Skilled employment is the primary route for most applicants.",
                "InvestmentAmount": 12000,
                "InvestmentCurrency": "EUR",
                "AdditionalInfo": "German language improves opportunities and integration.",
                "LivingCosts": {
                  "Housing": [],
                  "FamilyMonthly": "EUR 2800-3800",
                  "SingleMonthly": "EUR 1700-2400",
                  "Transport": "EUR 70-120 monthly pass",
                  "Utilities": "EUR 160-240",
                  "Recreation": "EUR 120-200",
                  "RentOneBedroom": "EUR 900-1400",
                  "RentThreeBedroom": "EUR 1700-2500",
                  "Internet": "EUR 35-55"
                },
                "Job": null,
                "Education": null,
                "PersonalityReport": null,
                "LanguageRequirement": null
              }
            },
            {
              "Country": "Netherlands",
              "Score": 81,
              "RecommendedVisaType": "Highly Skilled Migrant",
              "Data": {
                "MaritalStatusImpact": "Partner can be included if income thresholds are met.",
                "IranianMigrationRestrictions": [
                  "Processing can be longer due to security screening.",
                  "Extra documentation may be needed for financial history.",
                  "Employer compliance checks may add time."
                ],
                "InvestmentNotes": "Skilled employment paths are most common for new arrivals.",
                "InvestmentAmount": 18000,
                "InvestmentCurrency": "EUR",
                "AdditionalInfo": "English-friendly market with strong tech sector.",
                "LivingCosts": {
                  "Housing": [],
                  "FamilyMonthly": "EUR 3200-4200",
                  "SingleMonthly": "EUR 2100-2900",
                  "Transport": "EUR 100-140 monthly pass",
                  "Utilities": "EUR 180-260",
                  "Recreation": "EUR 160-220",
                  "RentOneBedroom": "EUR 1200-1800",
                  "RentThreeBedroom": "EUR 2200-3000",
                  "Internet": "EUR 40-60"
                },
                "Job": null,
                "Education": null,
                "PersonalityReport": null,
                "LanguageRequirement": null
              }
            },
            {
              "Country": "Sweden",
              "Score": 78,
              "RecommendedVisaType": "Work Permit",
              "Data": {
                "MaritalStatusImpact": "Family members can join with proof of support.",
                "IranianMigrationRestrictions": [
                  "Security screening can extend processing times.",
                  "Extra documentation may be required for identity checks.",
                  "Some banking services may require added verification."
                ],
                "InvestmentNotes": "Work permits typically do not require major investment.",
                "InvestmentAmount": 10000,
                "InvestmentCurrency": "SEK",
                "AdditionalInfo": "High quality of life but higher taxes.",
                "LivingCosts": {
                  "Housing": [],
                  "FamilyMonthly": "SEK 32000-42000",
                  "SingleMonthly": "SEK 21000-28000",
                  "Transport": "SEK 900-1100 monthly pass",
                  "Utilities": "SEK 900-1300",
                  "Recreation": "SEK 800-1200",
                  "RentOneBedroom": "SEK 9000-14000",
                  "RentThreeBedroom": "SEK 16000-23000",
                  "Internet": "SEK 350-450"
                },
                "Job": null,
                "Education": null,
                "PersonalityReport": null,
                "LanguageRequirement": null
              }
            }
          ]
        }
        """;
    }

    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        public SequenceHandler(IEnumerable<HttpResponseMessage> responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(_responses.Dequeue());
        }
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

    private sealed class TestImmigrationAiQueryHandler : ImmigrationAiQueryHandler
    {
        public TestImmigrationAiQueryHandler(
            IHttpClientFactory httpClientFactory,
            IOptions<GroqOptions> options,
            Microsoft.Extensions.Logging.ILogger<ImmigrationAiQueryHandler> logger,
            SequenceHandler handler)
            : base(httpClientFactory, options, logger)
        {
            SequenceHandler = handler;
        }

        public int CallCount => SequenceHandler.CallCount;

        private SequenceHandler SequenceHandler { get; }
    }
}
