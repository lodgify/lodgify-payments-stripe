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

    public WebhooksController(ISender mediatorSender)
    {
        _mediatorSender = mediatorSender;
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
        var json = await reader.ReadToEndAsync(cancel);
        if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignature))
            return BadRequest("Stripe-Signature Header is missing");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "_webhookSecret");
            switch (stripeEvent.Type)
            {
                case Events.AccountUpdated:
                    var account = (Account)stripeEvent.Data.Object;
                    await _mediatorSender.Send(new AccountUpdatedCommand(account.Id, account.ChargesEnabled, account.DetailsSubmitted, json, stripeEvent.Id), cancel);
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
    }
}