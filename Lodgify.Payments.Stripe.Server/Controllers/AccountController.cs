using Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;
using Lodgify.Payments.Stripe.Server.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lodgify.Payments.Stripe.Server.Controllers;

[ApiController]
[Route("api/v1/accounts")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateAccountCommand(request.Country, request.Email), cancellationToken);
        return Ok(response);
    }
}