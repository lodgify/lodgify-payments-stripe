using FluentAssertions;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Collections;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Configurations;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Factories;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Mocks;
using Lodgify.Payments.Stripe.Server.IntegrationTests.Shared;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.IntegrationTests.Controllers.v1;

[Collection(nameof(StripeCollection))]
public class StripeAccountSessionControllerTests : StripeIntegration
{
    private const string RequestBaseUrl = "api/v1/account-sessions";

    public StripeAccountSessionControllerTests(CustomWebApplicationFactory<StripeTestConfiguration> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Post_ShouldReturnOk_And_ClientSecretSholdBeCreated_WhenAccountSessionWasCreated()
    {
        //Arrange
        const int accountId = 4;
    
        var stripeAccountId = await CreateAccountInStripe(accountId);
    
        //Act
        var createAccountSessionResponse = await PostAsync(accountId, RequestBaseUrl, new CreateAccountSessionRequest(stripeAccountId));
    
        // Assert
        createAccountSessionResponse.IsSuccessStatusCode.Should().BeTrue();
    
        var accountSessionResponse = await DeserializeResponse<CreateAccountSessionResponse>(createAccountSessionResponse);
        accountSessionResponse.Should().NotBeNull();
        accountSessionResponse.ClientSecret.Should().NotBeNullOrEmpty();
    
        await AssertSessionAccountCreated(accountSessionResponse.StripeAccountId, accountSessionResponse.ClientSecret);
        await RemoveAccountFromStripe(stripeAccountId);
    }

    private async Task RemoveAccountFromStripe(string stripeAccountId)
    {
        var service = new AccountService(new StripeClient(apiBase: Configuration["StripeSettings:ApiBase"], apiKey: Configuration["StripeSettings:ApiKey"]));
        await service.DeleteAsync(stripeAccountId);
    }

    private async Task<string> CreateAccountInStripe(int accountId)
    {
        var createAccountResponse = await PostAsync(accountId, "api/v1/accounts", new CreateAccountRequest(PredefinedMocks.CountryMock.Us, PredefinedMocks.EmailMock.User1));
        var response = await DeserializeResponse<CreateAccountSessionResponse>(createAccountResponse);
        return response.StripeAccountId;
    }

    private async Task AssertSessionAccountCreated(string stripeAccountId, string clientSecret)
    {
        var accountSession = await DbContext.Set<Lodgify.Payments.Stripe.Domain.AccountSessions.AccountSession>().Where(x => x.StripeAccountId == stripeAccountId && x.ClientSecret == clientSecret).SingleAsync();
        accountSession.Should().NotBeNull();
    }
}