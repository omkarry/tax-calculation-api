using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeTaxCalculation.Data.Migrations
{
    public partial class Tax_details_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryDetails_FinancialYear_FinancialYearId",
                table: "SalaryDetails");

            migrationBuilder.DropIndex(
                name: "IX_SalaryDetails_EmployeeId_FinantialYearId",
                table: "SalaryDetails");

            migrationBuilder.DropColumn(
                name: "FinantialYearId",
                table: "SalaryDetails");

            migrationBuilder.AlterColumn<int>(
                name: "FinancialYearId",
                table: "SalaryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TaxDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    RegimeType = table.Column<int>(type: "int", nullable: false),
                    TaxPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxDetails_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDetails_EmployeeId_FinancialYearId",
                table: "SalaryDetails",
                columns: new[] { "EmployeeId", "FinancialYearId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_EmployeeId_FinancialYearId_RegimeType",
                table: "TaxDetails",
                columns: new[] { "EmployeeId", "FinancialYearId", "RegimeType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_FinancialYearId",
                table: "TaxDetails",
                column: "FinancialYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryDetails_FinancialYear_FinancialYearId",
                table: "SalaryDetails",
                column: "FinancialYearId",
                principalTable: "FinancialYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryDetails_FinancialYear_FinancialYearId",
                table: "SalaryDetails");

            migrationBuilder.DropTable(
                name: "TaxDetails");

            migrationBuilder.DropIndex(
                name: "IX_SalaryDetails_EmployeeId_FinancialYearId",
                table: "SalaryDetails");

            migrationBuilder.AlterColumn<int>(
                name: "FinancialYearId",
                table: "SalaryDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FinantialYearId",
                table: "SalaryDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDetails_EmployeeId_FinantialYearId",
                table: "SalaryDetails",
                columns: new[] { "EmployeeId", "FinantialYearId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryDetails_FinancialYear_FinancialYearId",
                table: "SalaryDetails",
                column: "FinancialYearId",
                principalTable: "FinancialYear",
                principalColumn: "Id");
        }
    }
}
