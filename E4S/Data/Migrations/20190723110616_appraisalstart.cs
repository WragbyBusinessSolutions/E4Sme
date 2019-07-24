using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class appraisalstart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appraisals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppraisalTitle = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appraisals", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appraisals");
        }
    }
}
