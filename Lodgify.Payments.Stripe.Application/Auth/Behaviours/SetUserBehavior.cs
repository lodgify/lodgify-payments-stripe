using Lodgify.Extensions.AspNetCore.Cqrs.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lodgify.Payments.Stripe.Application.Auth.Behaviours;

public class SetUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SetUserBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancel)
    {
        if (request is not ILodgifyAccountIdentified accountRequest || _httpContextAccessor.HttpContext?.User.Claims == null)
        {
            return await next();
        }

        var lodgifyIdentity = LodgifyAccountBuilder.GetLodgifyIdentity(_httpContextAccessor.HttpContext.User);
        accountRequest.Account = lodgifyIdentity.Account;

        return await next();
    }
}