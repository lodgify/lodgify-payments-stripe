using Lodgify.Extensions.AspNetCore.Cqrs.Abstractions;
using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface IIdentifiedCommand<out TResponse> : IRequest<TResponse>, ILodgifyAccountIdentified; 

public interface IIdentifiedCommand : IRequest, ILodgifyAccountIdentified;