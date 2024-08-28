using System.Security.Claims;
using Lodgify.Authentication.Constants;
using Lodgify.Extensions.AspNetCore.Cqrs.Abstractions;
using Lodgify.Extensions.Primitives.Identity;

namespace Lodgify.Payments.Stripe.Application.Auth;

public class LodgifyAccountBuilder
{
    static readonly HashSet<string> _lodgifyClaims = new HashSet<string>
    {
        LodgifyClaims.PropertyOwnerId,
        LodgifyClaims.SubOwnerId,
        LodgifyClaims.IsAdmin,
        LodgifyClaims.IsImpersonating,
        LodgifyClaims.AdminLoginId
    };

    public static LodgifyAccountIdentity GetLodgifyIdentity(ClaimsPrincipal identity)
    {
        int? propertyOwnerId = null;
        int? subOwnerId = null;
        int? adminId = null;
        bool isAdmin = false;
        bool isImpersonating = false;

        foreach (var claim in identity.Claims)
        {
            if (!_lodgifyClaims.Contains(claim.Type))
                continue;

            if (LodgifyClaims.PropertyOwnerId.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(claim.Value, out var poid))
                    propertyOwnerId = poid;
                continue;
            }

            if (LodgifyClaims.SubOwnerId.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(claim.Value, out var soid))
                    subOwnerId = soid;
                continue;
            }

            if (LodgifyClaims.IsAdmin.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
            {
                bool.TryParse(claim.Value, out isAdmin);
                continue;
            }

            if (LodgifyClaims.IsImpersonating.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
            {
                bool.TryParse(claim.Value, out isImpersonating);
                continue;
            }

            if (LodgifyClaims.AdminLoginId.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(claim.Value, out var aid))
                    adminId = aid;
                continue;
            }
        }

        if (propertyOwnerId.HasValue)
        {
            var account = new LodgifyAccount(propertyOwnerId.Value, subOwnerId ?? propertyOwnerId.Value, (isImpersonating ? adminId : null));
            return new LodgifyAccountIdentity(account, isAdmin, isImpersonating);
        }

        return LodgifyAccountIdentity.Anonymous;
    }

    protected bool SetAccount<T>(T obj, LodgifyAccount account)
    {
        if (obj is ILodgifyAccountIdentified identified)
        {
            if (account.AccountId != 0)
            {
                identified.Account = account;
                return true;
            }

            return false;
        }

        return true;
    }
}