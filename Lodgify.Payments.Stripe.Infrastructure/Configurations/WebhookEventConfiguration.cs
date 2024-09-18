using System.Diagnostics.CodeAnalysis;
using Lodgify.Payments.Stripe.Domain.WebhookEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lodgify.Payments.Stripe.Infrastructure.Configurations;

public class WebhookEventConfiguration : IEntityTypeConfiguration<WebhookEvent>
{
    public void Configure(EntityTypeBuilder<WebhookEvent> builder)
    {
        builder.ToTable("WebhookEvent");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.WebhookEventStripeId).IsRequired();
        builder.Property(p => p.RawEventData).HasColumnType("jsonb").IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.HasIndex(p => p.WebhookEventStripeId);
    }
}