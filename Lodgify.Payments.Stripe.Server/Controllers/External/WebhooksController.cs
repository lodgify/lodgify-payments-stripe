using System.Text;
using System.Text.Json;
using Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Lodgify.Payments.Stripe.Server.Controllers.External;

[ApiController]
[Route("external/webhooks/stripe")]
public class WebhooksController : Controller
{
    private readonly ISender _mediatorSender;
    private readonly IEventMapper _eventMapper;

    public WebhooksController(ISender mediatorSender, IEventMapper eventMapper)
    {
        _mediatorSender = mediatorSender;
        _eventMapper = eventMapper;
    }

    /// <summary>
    /// Receives webhooks with updates about users, connected accounts, devices, and keycodes from Seam.
    /// </summary>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HandleEvent([FromBody] JsonElement body, CancellationToken cancel)
    {
        using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
        var json = await reader.ReadToEndAsync();
        if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignature))
            return BadRequest("Stripe-Signature Header is missing");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], "_webhookSecret");
            switch (stripeEvent.Type)
            {
                case Events.AccountUpdated:
                    var account = (Account)stripeEvent.Data.Object;
                    //to nie powinien byc event tylko command
                    await _mediatorSender.Send(new AccountUpdatedCommand(account.Id, account.ChargesEnabled, account.DetailsSubmitted, account.Created), cancel);
                    break;
                default:
                    throw new NotSupportedException($"Event of type {stripeEvent.Type} not supported.");
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }        

        return Ok();
    }
}