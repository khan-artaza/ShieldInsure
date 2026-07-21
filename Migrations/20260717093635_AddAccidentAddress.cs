using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Insure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccidentAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccidentAddress",
                table: "Claims",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccidentAddress",
                table: "Claims");
        }
    }
}
