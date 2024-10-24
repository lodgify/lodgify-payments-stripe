using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lodgify.Payments.Stripe.Infrastructure;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Fixtures;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    protected readonly BaseFixture Factory;

    protected BaseIntegrationTest(BaseFixture factory)
    {
        Factory = factory;
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
        await using var scope = Factory.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    protected async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        return await dbContext.Set<TEntity>().FirstAsync(predicate);
    }
    
    protected async Task<TEntity?> GetSingleOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        return await dbContext.Set<TEntity>().SingleOrDefaultAsync(predicate);
    }

    protected async Task<List<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        await using var scope = Factory.Services.CreateAsyncScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
        return await dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }
}