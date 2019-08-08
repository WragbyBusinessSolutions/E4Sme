using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class expenseTypeo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperatingExpenses",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    ExpenseId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    ProductService = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatingExpenses_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperatingExpenses_ExpenseId",
                table: "OperatingExpenses",
                column: "ExpenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperatingExpenses");
        }
    }
}
