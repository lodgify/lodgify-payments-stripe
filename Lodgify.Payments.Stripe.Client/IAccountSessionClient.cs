using System.Threading;
using System.Threading.Tasks;
using Lodgify.Architecture.HttpClient.Attributes;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Client.Constans;

namespace Lodgify.Payments.Stripe.Client;

[HttpClient(ClientName = PaymentStripeConstans.HttpClientName, MetricsPrefix = PaymentStripeConstans.MetricsPrefix, UseResponseWrapper = false)]
public interface IAccountSessionClient
{
    [ApiEndpoint("POST", "/api/v1/account-sessions", Name = "CreateAccountSession")]
    Task<CreateAccountSessionResponse> CreateAccountSessionAsync(
        [HeaderValue(PaymentStripeConstans.PropertyOwnerIdHeaderName)]
        int propertyOwnerId,
        [HeaderValue(PaymentStripeConstans.SubOwnerIdHeaderName)]
        int? subOwnerId,
        [RequestBody] CreateAccountSessionRequest request,
        CancellationToken cancel);
}