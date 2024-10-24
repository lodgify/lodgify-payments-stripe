﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;

public abstract class BaseIntegrationTest
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _httpClient;
    private readonly BaseFixture _factory;
    protected DbContext DbContext => _factory.DbContext;

    protected BaseIntegrationTest(BaseFixture factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
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

    protected async Task<TResponse?> DeserializeResponse<TResponse>(HttpResponseMessage response)
        where TResponse : class
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(content, _jsonSerializerOptions);
    }

    protected async Task Insert<TEntity>(TEntity entity) where TEntity : class
    {
        await _factory.DbContext.AddAsync(entity);
        await _factory.DbContext.SaveChangesAsync();
    }
}