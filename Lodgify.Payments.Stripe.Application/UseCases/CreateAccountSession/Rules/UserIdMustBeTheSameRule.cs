using Lodgify.Payments.Stripe.Application.BuildingBlocks;

namespace Lodgify.Payments.Stripe.Application.UseCases.CreateAccountSession.Rules;

public class UserIdMustBeTheSameRule : IBusinessRule
{
    private readonly int _currentUserId;
    private readonly int? _accountUserId;

    public UserIdMustBeTheSameRule(int currentUserId, int? accountUserId)
    {
        _accountUserId = accountUserId;
        _currentUserId = currentUserId == 0 ? throw new ArgumentNullException(nameof(accountUserId)) : currentUserId;
    }

    public string Message => "The current userID must be the same as the userID of account.";

    public bool IsBroken()
    {
        if (_accountUserId == null)
        {
            return true;
        }

        return _currentUserId != _accountUserId;
    }
}