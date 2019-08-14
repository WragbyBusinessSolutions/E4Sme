using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceRecords",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    InvoiceNo = table.Column<int>(nullable: false),
                    QuoteNo = table.Column<int>(nullable: false),
                    CustomerId = table.Column<Guid>(nullable: false),
                    SubTotal = table.Column<float>(nullable: false),
                    Tax = table.Column<float>(nullable: false),
                    Total = table.Column<float>(nullable: false),
                    InvoiceTitle = table.Column<string>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ProductServiceId = table.Column<Guid>(nullable: false),
                    InvoiceRecordId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<float>(nullable: false),
                    UnitCost = table.Column<float>(nullable: false),
                    TotalCost = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_InvoiceRecords_InvoiceRecordId",
                        column: x => x.InvoiceRecordId,
                        principalTable: "InvoiceRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_ProductServices_ProductServiceId",
                        column: x => x.ProductServiceId,
                        principalTable: "ProductServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceRecordId",
                table: "InvoiceItems",
                column: "InvoiceRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ProductServiceId",
                table: "InvoiceItems",
                column: "ProductServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "InvoiceRecords");
        }
    }
}
