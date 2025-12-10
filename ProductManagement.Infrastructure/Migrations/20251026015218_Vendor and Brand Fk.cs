using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VendorandBrandFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrandVendor",
                columns: table => new
                {
                    BrandsBrandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorsVendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandVendor", x => new { x.BrandsBrandId, x.VendorsVendorId });
                    table.ForeignKey(
                        name: "FK_BrandVendor_Brands_BrandsBrandId",
                        column: x => x.BrandsBrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandVendor_Vendors_VendorsVendorId",
                        column: x => x.VendorsVendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandVendor_VendorsVendorId",
                table: "BrandVendor",
                column: "VendorsVendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandVendor");
        }
    }
}
