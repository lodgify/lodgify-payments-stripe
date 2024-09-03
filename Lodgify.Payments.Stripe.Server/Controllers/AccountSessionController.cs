using Lodgify.Authentication.Constants;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;
using Lodgify.Payments.Stripe.Server.Requests;
using Lodgify.Payments.Stripe.Server.Responses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodgify.Payments.Stripe.Server.Controllers;

[Authorize(nameof(LodgifyAuthPolicies.AnySubscribed))]
[ApiController]
[Route("api/v1/account-sessions")]
public class AccountSessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountSessionController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Create account session
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateAccountSessionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateAccountSessionResponse>> CreateAccountSession(CreateAccountSessionRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateAccountSessionCommand(request.StripeAccountId), cancellationToken);
        return Ok(response.Adapt<CreateAccountSessionResponse>());
    }
}