using System.Text;
using FluentAssertions;
using Lodgify.Payments.Stripe.Application.UseCases.UpdateAccount;
using Lodgify.Payments.Stripe.Infrastructure.Settings;
using Lodgify.Payments.Stripe.Server.Controllers.v1.External;
using Lodgify.Payments.Stripe.Server.Tests.Shared;
using Lodgify.Payments.Stripe.Server.UnitTests.Payloads;
using Lodgify.Payments.Stripe.Shared.Payloads;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace Lodgify.Payments.Stripe.Server.UnitTests.Controllers.v1.Externall;

public class WebhookControllerTests
{
    private readonly ISender _mediatorSenderMock;
    private readonly IOptions<StripeSettings> _stripeSettingsMock;
    private readonly WebhooksController _controller;

    public WebhookControllerTests()
    {
        _mediatorSenderMock = Substitute.For<ISender>();
        _stripeSettingsMock = Substitute.For<IOptions<StripeSettings>>();
        _stripeSettingsMock.Value.Returns(new StripeSettings { WebhookSecret = "test_webhook_secret" });
        _controller = new WebhooksController(_mediatorSenderMock, _stripeSettingsMock, Substitute.For<ILogger<WebhooksController>>());
    }

    [Fact]
    public async Task HandleEvent_ShouldReturnBadRequest_WhenStripeSignatureHeaderIsMissing()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.HandleEvent(CancellationToken.None);

        // Assert
        result.Should().BeAssignableTo<BadRequestObjectResult>();
        var badRequestResult = (BadRequestObjectResult)result;
        badRequestResult.Value.Should().Be("Stripe-Signature Header is missing");
    }

    [Fact]
    public async Task HandleEvent_ShouldReturnOk_WhenEventIsHandledSuccessfully()
    {
        // Arrange
        var payload = PayloadLoader.Load(PayloadType.AccountUpdatedChargesEnabledDetailsSubmitted);
        var stripeSignature = StripeSignatureBuilder.GenerateSignature(payload, _stripeSettingsMock.Value.WebhookSecret);

        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        context.Request.Headers["Stripe-Signature"] = stripeSignature;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.HandleEvent(CancellationToken.None);

        // Assert
        result.Should().BeAssignableTo<OkResult>();
        await _mediatorSenderMock.Received(1).Send(Arg.Any<AccountUpdatedCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleEvent_ShouldReturnBadRequest_WhenStripeExceptionIsThrown()
    {
        // Arrange
        var payload = PayloadLoader.Load(PayloadType.AccountUpdatedStripeExceptionMaker);
        var stripeSignature = StripeSignatureBuilder.GenerateSignature(payload, _stripeSettingsMock.Value.WebhookSecret);

        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        context.Request.Headers["Stripe-Signature"] = stripeSignature;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.HandleEvent(CancellationToken.None);


        // Assert
        result.Should().BeAssignableTo<BadRequestResult>();
        await _mediatorSenderMock.DidNotReceive().Send(Arg.Any<AccountUpdatedCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleEvent_ShouldReturnBadRequest_WhenNotSupportedExceptionIsThrown()
    {
        // Arrange
        var payload = PayloadLoader.Load(PayloadType.NotSupportedEventType);
        var stripeSignature = StripeSignatureBuilder.GenerateSignature(payload, _stripeSettingsMock.Value.WebhookSecret);

        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(payload));
        context.Request.Headers["Stripe-Signature"] = stripeSignature;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act && Assert
        Func<Task> act = async () => await _controller.HandleEvent(CancellationToken.None);
        await act.Should().ThrowAsync<NotSupportedException>();
    }
}