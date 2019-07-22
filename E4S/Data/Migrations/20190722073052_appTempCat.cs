using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace E4S.Data.Migrations
{
    public partial class appTempCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppraisalTemplateCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppraisalCategoryId = table.Column<Guid>(nullable: false),
                    AppraisalTemplateId = table.Column<Guid>(nullable: false),
                    AssignTo = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    Weight = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalTemplateCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalTemplateCategories_AppraisalCategories_AppraisalCategoryId",
                        column: x => x.AppraisalCategoryId,
                        principalTable: "AppraisalCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalTemplateCategories_AppraisalCategoryId",
                table: "AppraisalTemplateCategories",
                column: "AppraisalCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalTemplateCategories");
        }
    }
}
