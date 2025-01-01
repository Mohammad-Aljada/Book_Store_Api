using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book_Store_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Categories_CategoryId1",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CategoryId1",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId1",
                table: "Books",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Categories_CategoryId1",
                table: "Books",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
