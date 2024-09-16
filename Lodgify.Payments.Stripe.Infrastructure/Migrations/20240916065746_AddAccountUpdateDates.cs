using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodgify.Payments.Stripe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountUpdateDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ChargesEnabledAt",
                table: "Account",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DetailsSubmittedAt",
                table: "Account",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargesEnabledAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DetailsSubmittedAt",
                table: "Account");
        }
    }
}
