using Lodgify.Payments.Stripe.Domain.AccountHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodgify.Payments.Stripe.Infrastructure.Configurations;

public class AccountHistoryConfiguration : IEntityTypeConfiguration<AccountHistory>
{
    public void Configure(EntityTypeBuilder<AccountHistory> builder)
    {
        builder.ToTable("AccountHistory");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.AccountId).IsRequired();
        builder.Property(p => p.PropertyName).IsRequired();
        builder.Property(p => p.PropertyValue).IsRequired();
        builder.Property(p => p.SetAt).IsRequired();
        builder.Property(p => p.WebhookEventStripeId).IsRequired();
    }
}