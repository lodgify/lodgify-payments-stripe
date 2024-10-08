using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CacheCow.Client;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using WireMock.Admin.Mappings;
using WireMock.Client;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

[CollectionDefinition(nameof(PaymentsCollection))]
public class PaymentsCollection : ICollectionFixture<CustomWebApplicationFactory>;

[Collection(nameof(PaymentsCollection))]
public abstract class BaseIntegrationTest
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _httpClient;
    private readonly IWireMockAdminApi _wireMockClient;
    private readonly CustomWebApplicationFactory _factory;
    protected DbContext DbContext => _factory.DbContext;

    protected BaseIntegrationTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateDefaultClient(new CachingHandler() { });
        _wireMockClient = factory.WiremockClient;
        _factory = factory;
    }

    private void SetAuthHeader(int accountId)
        => _httpClient.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", AuthHelper.GenerateToken(accountId));

    protected Task<HttpResponseMessage> PostAsync(int accountId, string requestUrl, object? content)
    {
        SetAuthHeader(accountId);
        return _httpClient.PostAsync(requestUrl, new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));
    }

    protected Task<HttpResponseMessage> GetAsync(int accountId, string requestUrl)
    {
        SetAuthHeader(accountId);
        return _httpClient.GetAsync(requestUrl);
    }

    protected async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        where T : class
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
    }

    protected async Task Insert<T>(T entity) where T : class
    {
        await _factory.DbContext.AddAsync(entity);
        await _factory.DbContext.SaveChangesAsync();
    }

    protected async Task UseMockMapping(MappingModel mappingModel)
    {
        await _wireMockClient.PostMappingAsync(mappingModel);
    }
}