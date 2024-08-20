using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccount;

public sealed record CreateAccountCommand(string Country, string Email, string FeePayer, string LossPayments, string DashboardType) : ICommand<CreateAccountResponse>;