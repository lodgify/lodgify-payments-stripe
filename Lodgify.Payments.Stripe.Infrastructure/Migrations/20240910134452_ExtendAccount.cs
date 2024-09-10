using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodgify.Payments.Stripe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChargesEnabled",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DetailsSubmitted",
                table: "Account",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargesEnabled",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "DetailsSubmitted",
                table: "Account");
        }
    }
}
