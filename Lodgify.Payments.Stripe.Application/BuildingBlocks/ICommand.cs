using Lodgify.Extensions.AspNetCore.Cqrs.Abstractions;
using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

public interface ICommand<out TResponse> : IRequest<TResponse>, ILodgifyAccountIdentified; 

public interface ICommand : IRequest, ILodgifyAccountIdentified;