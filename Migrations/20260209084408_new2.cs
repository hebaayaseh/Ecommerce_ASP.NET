using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_ASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class new2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "payments",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_userId",
                table: "payments",
                newName: "IX_payments_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Users_UsersId",
                table: "payments",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Users_UsersId",
                table: "payments");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "payments",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_payments_UsersId",
                table: "payments",
                newName: "IX_payments_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Users_userId",
                table: "payments",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
