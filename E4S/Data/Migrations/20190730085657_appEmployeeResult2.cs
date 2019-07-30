using Microsoft.EntityFrameworkCore.Migrations;

namespace E4S.Data.Migrations
{
    public partial class appEmployeeResult2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppraisalEmployeeResults_AppraisalId",
                table: "AppraisalEmployeeResults",
                column: "AppraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalEmployeeResults_EmployeeDetailId",
                table: "AppraisalEmployeeResults",
                column: "EmployeeDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalEmployeeResults_Appraisals_AppraisalId",
                table: "AppraisalEmployeeResults",
                column: "AppraisalId",
                principalTable: "Appraisals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppraisalEmployeeResults_EmployeeDetails_EmployeeDetailId",
                table: "AppraisalEmployeeResults",
                column: "EmployeeDetailId",
                principalTable: "EmployeeDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalEmployeeResults_Appraisals_AppraisalId",
                table: "AppraisalEmployeeResults");

            migrationBuilder.DropForeignKey(
                name: "FK_AppraisalEmployeeResults_EmployeeDetails_EmployeeDetailId",
                table: "AppraisalEmployeeResults");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalEmployeeResults_AppraisalId",
                table: "AppraisalEmployeeResults");

            migrationBuilder.DropIndex(
                name: "IX_AppraisalEmployeeResults_EmployeeDetailId",
                table: "AppraisalEmployeeResults");
        }
    }
}
