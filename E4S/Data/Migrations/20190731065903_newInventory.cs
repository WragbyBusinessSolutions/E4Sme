using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class newInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewInventories",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ProductServiceId = table.Column<Guid>(nullable: false),
                    UnitPrice = table.Column<float>(nullable: false),
                    TotalPrice = table.Column<float>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    VendorId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewInventories_ProductServices_ProductServiceId",
                        column: x => x.ProductServiceId,
                        principalTable: "ProductServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockRecords",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ProductServiceId = table.Column<Guid>(nullable: false),
                    AllTimeQuantity = table.Column<float>(nullable: false),
                    QuantitySold = table.Column<float>(nullable: false),
                    QuantityRemain = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockRecords_ProductServices_ProductServiceId",
                        column: x => x.ProductServiceId,
                        principalTable: "ProductServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewInventories_ProductServiceId",
                table: "NewInventories",
                column: "ProductServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockRecords_ProductServiceId",
                table: "StockRecords",
                column: "ProductServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewInventories");

            migrationBuilder.DropTable(
                name: "StockRecords");
        }
    }
}
