using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GUI_2.Migrations
{
    /// <inheritdoc />
    public partial class boolbeischicht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Mitternachtsarbeit",
                table: "Schichten",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Mitternachtsarbeit",
                table: "Schichten",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
