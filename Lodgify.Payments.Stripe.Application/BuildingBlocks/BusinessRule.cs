namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public class BusinessRule
{
    public static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}