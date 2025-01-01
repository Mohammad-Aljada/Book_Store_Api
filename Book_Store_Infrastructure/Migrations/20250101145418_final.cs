using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Store_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "InvoiceItems",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "InvoiceItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "BookTitle",
                table: "InvoiceItems");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "InvoiceItems",
                newName: "UnitPrice");
        }
    }
}
