using Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession;
using Lodgify.Payments.Stripe.Server.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lodgify.Payments.Stripe.Server.Controllers;

[ApiController]
[Route("api/v1/accountsessions")]
public class AccountSessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountSessionController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountSession(CreateAccountSessionRequest request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateAccountSessionCommand(request.StripeAccountId), cancellationToken);
        return Ok(response);
    }
}