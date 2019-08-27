using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class accountOtherItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtherItems",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Item = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceOtherItems",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    OtherItemId = table.Column<Guid>(nullable: false),
                    InvoiceRecordId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceOtherItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceOtherItems_InvoiceRecords_InvoiceRecordId",
                        column: x => x.InvoiceRecordId,
                        principalTable: "InvoiceRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceOtherItems_OtherItems_OtherItemId",
                        column: x => x.OtherItemId,
                        principalTable: "OtherItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOtherItems_InvoiceRecordId",
                table: "InvoiceOtherItems",
                column: "InvoiceRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceOtherItems_OtherItemId",
                table: "InvoiceOtherItems",
                column: "OtherItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceOtherItems");

            migrationBuilder.DropTable(
                name: "OtherItems");
        }
    }
}
