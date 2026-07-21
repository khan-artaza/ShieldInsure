using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Final_Insure.Migrations
{
    /// <inheritdoc />
    public partial class AddFraudCheckColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CheckDate",
                table: "FraudChecks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsFraudulent",
                table: "FraudChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "FraudChecks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckDate",
                table: "FraudChecks");

            migrationBuilder.DropColumn(
                name: "IsFraudulent",
                table: "FraudChecks");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "FraudChecks");
        }
    }
}
