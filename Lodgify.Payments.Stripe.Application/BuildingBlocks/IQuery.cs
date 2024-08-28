using Lodgify.Extensions.AspNetCore.Cqrs.Abstractions;
using MediatR;

namespace Lodgify.Payments.Stripe.Application.BuildingBlocks;

 public interface IQuery<out TResult> : IRequest<TResult>, ILodgifyAccountIdentified;