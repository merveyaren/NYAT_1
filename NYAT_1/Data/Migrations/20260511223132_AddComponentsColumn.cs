using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NYAT_1.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Components",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Components",
                table: "Products");
        }
    }
}
