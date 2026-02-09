using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_ASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class newpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_payments_userId",
                table: "payments",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_userId",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "payments");
        }
    }
}
