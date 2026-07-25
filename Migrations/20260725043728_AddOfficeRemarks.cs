using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Insure.Migrations
{
    /// <inheritdoc />
    public partial class AddOfficeRemarks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficerRemarks",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficerRemarks",
                table: "Claims");
        }
    }
}
