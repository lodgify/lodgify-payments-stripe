using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodgify.Payments.Stripe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotNullableDatesInAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargesEnabledAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DetailsSubmittedAt",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "WebhookEventStripeId",
                table: "AccountHistory",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChargesEnabledSetAt",
                table: "Account",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Account",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DetailsSubmittedSetAt",
                table: "Account",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvent_WebhookEventStripeId",
                table: "WebhookEvent",
                column: "WebhookEventStripeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebhookEvent_WebhookEventStripeId",
                table: "WebhookEvent");

            migrationBuilder.DropColumn(
                name: "ChargesEnabledSetAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DetailsSubmittedSetAt",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "WebhookEventStripeId",
                table: "AccountHistory",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
