﻿using Lodgify.Payments.Stripe.Domain.Accounts;
using Lodgify.Payments.Stripe.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Lodgify.Payments.Stripe.Infrastructure;

public class PaymentDbContext : DbContext
{
    public DbSet<Account> Account { get; set; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.LogTo(Console.WriteLine)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}