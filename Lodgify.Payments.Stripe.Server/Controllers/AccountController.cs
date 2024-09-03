using Lodgify.Authentication.Constants;
using Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;
using Lodgify.Payments.Stripe.Application.UseCases.GetAccounts;
using Lodgify.Payments.Stripe.Server.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lodgify.Payments.Stripe.Server.Controllers;

[Authorize(nameof(LodgifyAuthPolicies.AnySubscribed))]
[ApiController]
[Route("api/v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Create account
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccount(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateAccountCommand(request.Country, request.Email), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Get already login user accounts
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of user accounts contains active, deleted, rejected etc.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(CreateAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAccountsResponse>> GetAccounts(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAccountsQuery(), cancellationToken);
        return Ok(response);
    }
}