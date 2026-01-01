using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class customAttforCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts");

            migrationBuilder.AddColumn<Guid>(
                name: "CartProductId",
                table: "CartProducts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts",
                column: "CartProductId");

            migrationBuilder.CreateTable(
                name: "CartProductCustomAttribute",
                columns: table => new
                {
                    CartProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductCustomAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProductCustomAttribute", x => new { x.ProductCustomAttributeId, x.CartProductId });
                    table.ForeignKey(
                        name: "FK_CartProductCustomAttribute_CartProducts_CartProductId",
                        column: x => x.CartProductId,
                        principalTable: "CartProducts",
                        principalColumn: "CartProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartProductCustomAttribute_ProductCustomAttributes_ProductCustomAttributeId",
                        column: x => x.ProductCustomAttributeId,
                        principalTable: "ProductCustomAttributes",
                        principalColumn: "ProductCustomAttributeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartProducts_CartId",
                table: "CartProducts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartProductCustomAttribute_CartProductId",
                table: "CartProductCustomAttribute",
                column: "CartProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProductCustomAttribute");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts");

            migrationBuilder.DropIndex(
                name: "IX_CartProducts_CartId",
                table: "CartProducts");

            migrationBuilder.DropColumn(
                name: "CartProductId",
                table: "CartProducts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartProducts",
                table: "CartProducts",
                columns: new[] { "CartId", "ProductId" });
        }
    }
}
