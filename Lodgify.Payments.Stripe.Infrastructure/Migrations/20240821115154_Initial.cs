using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lodgify.Payments.Stripe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    StripeAccountId = table.Column<string>(type: "text", nullable: false),
                    Dashboard = table.Column<string>(type: "text", nullable: false),
                    RequirementCollection = table.Column<string>(type: "text", nullable: false),
                    Fees = table.Column<string>(type: "text", nullable: false),
                    Losses = table.Column<string>(type: "text", nullable: false),
                    ControllerType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
