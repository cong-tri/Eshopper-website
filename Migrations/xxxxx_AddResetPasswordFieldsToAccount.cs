using Microsoft.EntityFrameworkCore.Migrations;

namespace Eshopper_website.Migrations;

public partial class AddResetPasswordFieldsToAccount : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ResetPasswordToken",
            table: "Accounts",
            type: "nvarchar(100)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ResetPasswordExpiry",
            table: "Accounts",
            type: "datetime2",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ResetPasswordToken",
            table: "Accounts");

        migrationBuilder.DropColumn(
            name: "ResetPasswordExpiry",
            table: "Accounts");
    }
} 