﻿using Lodgify.Authentication.Constants;
using Lodgify.Payments.Stripe.Api.Models.v1.Requests;
using Lodgify.Payments.Stripe.Api.Models.v1.Responses;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodgify.Payments.Stripe.Server.Controllers.v1;

[Authorize(nameof(LodgifyAuthPolicies.AnySubscribed))]
[ApiController]
[Route("api/v1/account-sessions")]
public class WireMockAccountSessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public WireMockAccountSessionController(IMediator mediator)
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
        var response = await _mediator.Send(new CreateAccountSessionIdentifiedCommand(request.StripeAccountId), cancellationToken);
        return Ok(response.Adapt<CreateAccountSessionResponse>());
    }
}