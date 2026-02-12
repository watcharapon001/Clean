using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCaseRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.DropForeignKey(
                name: "FK_su_user_db_employee_employee_id",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropForeignKey(
                name: "FK_su_user_org_su_organize_org_id",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropForeignKey(
                name: "FK_su_user_org_su_user_user_id",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropForeignKey(
                name: "FK_su_user_profile_su_profile_profile_id",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropForeignKey(
                name: "FK_su_user_profile_su_user_user_id",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_su_user_profile",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_su_user_org",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropPrimaryKey(
                name: "PK_su_user",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_su_profile",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_su_organize",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_db_employee",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_profile_profile_id",
                schema: "clean",
                table: "su_user_profile",
                newName: "ix_su_user_profile_profile_id");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_org_org_id",
                schema: "clean",
                table: "su_user_org",
                newName: "ix_su_user_org_org_id");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_username",
                schema: "clean",
                table: "su_user",
                newName: "ix_su_user_username");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_is_active",
                schema: "clean",
                table: "su_user",
                newName: "ix_su_user_is_active");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_employee_id",
                schema: "clean",
                table: "su_user",
                newName: "ix_su_user_employee_id");

            migrationBuilder.RenameIndex(
                name: "IX_su_user_email_normalized",
                schema: "clean",
                table: "su_user",
                newName: "ix_su_user_email_normalized");

            migrationBuilder.RenameIndex(
                name: "IX_su_profile_profile_code",
                schema: "clean",
                table: "su_profile",
                newName: "ix_su_profile_profile_code");

            migrationBuilder.RenameIndex(
                name: "IX_su_organize_org_code",
                schema: "clean",
                table: "su_organize",
                newName: "ix_su_organize_org_code");

            migrationBuilder.RenameIndex(
                name: "IX_db_employee_org_id_employee_code",
                schema: "clean",
                table: "db_employee",
                newName: "ix_db_employee_org_id_employee_code");

            migrationBuilder.RenameIndex(
                name: "IX_db_employee_org_id",
                schema: "clean",
                table: "db_employee",
                newName: "ix_db_employee_org_id");

            migrationBuilder.RenameIndex(
                name: "IX_db_employee_email",
                schema: "clean",
                table: "db_employee",
                newName: "ix_db_employee_email");

            migrationBuilder.AddPrimaryKey(
                name: "pk_su_user_profile",
                schema: "clean",
                table: "su_user_profile",
                columns: new[] { "user_id", "profile_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_su_user_org",
                schema: "clean",
                table: "su_user_org",
                columns: new[] { "user_id", "org_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_su_user",
                schema: "clean",
                table: "su_user",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_su_profile",
                schema: "clean",
                table: "su_profile",
                column: "profile_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_su_organize",
                schema: "clean",
                table: "su_organize",
                column: "org_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_db_employee",
                schema: "clean",
                table: "db_employee",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "fk_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_su_user_db_employee_employee_id",
                schema: "clean",
                table: "su_user",
                column: "employee_id",
                principalSchema: "clean",
                principalTable: "db_employee",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_su_user_org_su_organize_org_id",
                schema: "clean",
                table: "su_user_org",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_su_user_org_su_user_user_id",
                schema: "clean",
                table: "su_user_org",
                column: "user_id",
                principalSchema: "clean",
                principalTable: "su_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_su_user_profile_su_profile_profile_id",
                schema: "clean",
                table: "su_user_profile",
                column: "profile_id",
                principalSchema: "clean",
                principalTable: "su_profile",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_su_user_profile_su_user_user_id",
                schema: "clean",
                table: "su_user_profile",
                column: "user_id",
                principalSchema: "clean",
                principalTable: "su_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.DropForeignKey(
                name: "fk_su_user_db_employee_employee_id",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropForeignKey(
                name: "fk_su_user_org_su_organize_org_id",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropForeignKey(
                name: "fk_su_user_org_su_user_user_id",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropForeignKey(
                name: "fk_su_user_profile_su_profile_profile_id",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropForeignKey(
                name: "fk_su_user_profile_su_user_user_id",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropPrimaryKey(
                name: "pk_su_user_profile",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropPrimaryKey(
                name: "pk_su_user_org",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropPrimaryKey(
                name: "pk_su_user",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropPrimaryKey(
                name: "pk_su_profile",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropPrimaryKey(
                name: "pk_su_organize",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropPrimaryKey(
                name: "pk_db_employee",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_profile_profile_id",
                schema: "clean",
                table: "su_user_profile",
                newName: "IX_su_user_profile_profile_id");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_org_org_id",
                schema: "clean",
                table: "su_user_org",
                newName: "IX_su_user_org_org_id");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_username",
                schema: "clean",
                table: "su_user",
                newName: "IX_su_user_username");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_is_active",
                schema: "clean",
                table: "su_user",
                newName: "IX_su_user_is_active");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_employee_id",
                schema: "clean",
                table: "su_user",
                newName: "IX_su_user_employee_id");

            migrationBuilder.RenameIndex(
                name: "ix_su_user_email_normalized",
                schema: "clean",
                table: "su_user",
                newName: "IX_su_user_email_normalized");

            migrationBuilder.RenameIndex(
                name: "ix_su_profile_profile_code",
                schema: "clean",
                table: "su_profile",
                newName: "IX_su_profile_profile_code");

            migrationBuilder.RenameIndex(
                name: "ix_su_organize_org_code",
                schema: "clean",
                table: "su_organize",
                newName: "IX_su_organize_org_code");

            migrationBuilder.RenameIndex(
                name: "ix_db_employee_org_id_employee_code",
                schema: "clean",
                table: "db_employee",
                newName: "IX_db_employee_org_id_employee_code");

            migrationBuilder.RenameIndex(
                name: "ix_db_employee_org_id",
                schema: "clean",
                table: "db_employee",
                newName: "IX_db_employee_org_id");

            migrationBuilder.RenameIndex(
                name: "ix_db_employee_email",
                schema: "clean",
                table: "db_employee",
                newName: "IX_db_employee_email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_su_user_profile",
                schema: "clean",
                table: "su_user_profile",
                columns: new[] { "user_id", "profile_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_su_user_org",
                schema: "clean",
                table: "su_user_org",
                columns: new[] { "user_id", "org_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_su_user",
                schema: "clean",
                table: "su_user",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_su_profile",
                schema: "clean",
                table: "su_profile",
                column: "profile_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_su_organize",
                schema: "clean",
                table: "su_organize",
                column: "org_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_db_employee",
                schema: "clean",
                table: "db_employee",
                column: "employee_id");

            migrationBuilder.AddForeignKey(
                name: "FK_db_employee_su_organize_org_id",
                schema: "clean",
                table: "db_employee",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_su_user_db_employee_employee_id",
                schema: "clean",
                table: "su_user",
                column: "employee_id",
                principalSchema: "clean",
                principalTable: "db_employee",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_su_user_org_su_organize_org_id",
                schema: "clean",
                table: "su_user_org",
                column: "org_id",
                principalSchema: "clean",
                principalTable: "su_organize",
                principalColumn: "org_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_su_user_org_su_user_user_id",
                schema: "clean",
                table: "su_user_org",
                column: "user_id",
                principalSchema: "clean",
                principalTable: "su_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_su_user_profile_su_profile_profile_id",
                schema: "clean",
                table: "su_user_profile",
                column: "profile_id",
                principalSchema: "clean",
                principalTable: "su_profile",
                principalColumn: "profile_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_su_user_profile_su_user_user_id",
                schema: "clean",
                table: "su_user_profile",
                column: "user_id",
                principalSchema: "clean",
                principalTable: "su_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
