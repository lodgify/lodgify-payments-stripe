﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodgify.Payments.Stripe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountAddRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Account",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Account");
        }
    }
}
