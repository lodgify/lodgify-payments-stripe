using Lodgify.Payments.Stripe.Domain.AccountSessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodgify.Payments.Stripe.Infrastructure.Configurations;

public class AccountSessionConfiguration : IEntityTypeConfiguration<AccountSession>
{
    public void Configure(EntityTypeBuilder<AccountSession> builder)
    {
        builder.ToTable("AccountSession");
        
        builder.Property(p => p.StripeAccountId).IsRequired();
        builder.Property(p => p.ClientSecret).IsRequired();
    }
}