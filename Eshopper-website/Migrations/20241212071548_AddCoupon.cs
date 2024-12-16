using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopper_website.Migrations
{
    /// <inheritdoc />
    public partial class AddCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    COUP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    COUP_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUP_Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    COUP_Status = table.Column<int>(type: "int", nullable: false),
                    COUP_Quantity = table.Column<int>(type: "int", nullable: false),
                    COUP_DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    COUP_DateExpire = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.COUP_ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
