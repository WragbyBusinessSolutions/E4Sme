using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class expenseTypeottrans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cashflows",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    FlowType = table.Column<string>(nullable: true),
                    FlowTypeId = table.Column<Guid>(nullable: false),
                    DebitCredit = table.Column<string>(nullable: true),
                    BankId = table.Column<Guid>(nullable: false),
                    FlowDetails = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    Tax = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    TransactionType = table.Column<string>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: false),
                    DebitCredit = table.Column<string>(nullable: true),
                    BankId = table.Column<Guid>(nullable: false),
                    TransactionDetails = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cashflows");

            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
