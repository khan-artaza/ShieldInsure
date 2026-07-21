using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Insure.Migrations
{
    /// <inheritdoc />
    public partial class SurveyorDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccidentMatchesDescription",
                table: "Assessments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "Assessments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EvidencePhotoPath",
                table: "Assessments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GarageDetails",
                table: "Assessments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccidentMatchesDescription",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "EvidencePhotoPath",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "GarageDetails",
                table: "Assessments");
        }
    }
}
