using System.Threading;
using System.Threading.Tasks;
using Lodgify.Architecture.HttpClient.Attributes;
using Lodgify.Payments.Stripe.Api.Models.Requests;
using Lodgify.Payments.Stripe.Api.Models.Responses;
using Lodgify.Payments.Stripe.Client.Constans;

namespace Lodgify.Payments.Stripe.Client;

[HttpClient(ClientName = PaymentStripeConstans.HttpClientName, MetricsPrefix = PaymentStripeConstans.MetricsPrefix, UseResponseWrapper = false)]
public interface IPaymentStripeClient
{
    [ApiEndpoint("POST", "/api/v1/accounts", Name = "CreateAccount")]
    Task<CreateAccountResponse> CreateAccountAsync(
        [HeaderValue(PaymentStripeConstans.PropertyOwnerIdHeaderName)]
        int propertyOwnerId,
        [HeaderValue(PaymentStripeConstans.SubOwnerIdHeaderName)]
        int? subOwnerId,
        [RequestBody] CreateAccountRequest request,
        CancellationToken cancel);

    [ApiEndpoint("GET", "/api/v1/accounts", Name = "GetAccounts")]
    Task<GetAccountsResponse> GetAccountsAsync(
        [HeaderValue(PaymentStripeConstans.PropertyOwnerIdHeaderName)]
        int propertyOwnerId,
        [HeaderValue(PaymentStripeConstans.SubOwnerIdHeaderName)]
        int? subOwnerId,
        CancellationToken cancel);

    [ApiEndpoint("POST", "/api/v1/account-sessions", Name = "CreateAccountSession")]
    Task<CreateAccountSessionResponse> CreateAccountSessionAsync(
        [HeaderValue(PaymentStripeConstans.PropertyOwnerIdHeaderName)]
        int propertyOwnerId,
        [HeaderValue(PaymentStripeConstans.SubOwnerIdHeaderName)]
        int? subOwnerId,
        [RequestBody] CreateAccountSessionRequest request,
        CancellationToken cancel);
}