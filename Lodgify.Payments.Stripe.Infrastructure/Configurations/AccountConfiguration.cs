using System.Diagnostics.CodeAnalysis;
using Lodgify.Payments.Stripe.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodgify.Payments.Stripe.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");

        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.Email).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.StripeAccountId).IsRequired();
        builder.Property(p => p.Dashboard);
        builder.Property(p => p.RequirementCollection);
        builder.Property(p => p.Fees);
        builder.Property(p => p.Losses);
        builder.Property(p => p.ControllerType);
        builder.Property(p => p.ChargesEnabled);
        builder.Property(p => p.DetailsSubmitted);
    }
}