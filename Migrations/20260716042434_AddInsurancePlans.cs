using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Final_Insure.Migrations
{
    /// <inheritdoc />
    public partial class AddInsurancePlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChassisNumber",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRC",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InsurancePlans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasePremium = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    CoverageAmount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePlans", x => x.PlanId);
                });

            migrationBuilder.InsertData(
                table: "InsurancePlans",
                columns: new[] { "PlanId", "BasePremium", "CoverageAmount", "Description", "IsActive", "PlanName", "PlanType" },
                values: new object[,]
                {
                    { 1, 7200m, 800000m, "Covers damage to others (Third-Party liability only). Required by law.", true, "Third-Party Mandatory", "Vehicle" },
                    { 2, 18500m, 2500000m, "Covers damage to your car + others (Natural disasters, theft, accidents).", true, "Comprehensive Car Insurance", "Vehicle" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsurancePlans");

            migrationBuilder.DropColumn(
                name: "ChassisNumber",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "VehicleRC",
                table: "Policies");
        }
    }
}
