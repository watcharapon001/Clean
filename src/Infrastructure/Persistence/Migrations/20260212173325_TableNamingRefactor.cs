using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TableNamingRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.AddForeignKey(
                name: "fk_db_employee_organizes_org_id",
                schema: "clean",
                table: "db_employee",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_db_employee_organizes_org_id",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.AddForeignKey(
                name: "fk_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
