using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropColumn(
                name: "joined_at",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "clean",
                table: "su_user",
                newName: "upd_date");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "clean",
                table: "su_organize",
                newName: "upd_date");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                schema: "clean",
                table: "db_employee",
                newName: "upd_date");

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "su_user_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "su_user_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "su_user_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "upd_date",
                schema: "clean",
                table: "su_user_profile",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "su_user_org",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "su_user_org",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "su_user_org",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "upd_date",
                schema: "clean",
                table: "su_user_org",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "su_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "su_user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "su_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "su_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "su_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "su_profile",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "upd_date",
                schema: "clean",
                table: "su_profile",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "su_organize",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "su_organize",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "su_organize",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cr_by",
                schema: "clean",
                table: "db_employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "cr_date",
                schema: "clean",
                table: "db_employee",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "upd_by",
                schema: "clean",
                table: "db_employee",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropColumn(
                name: "upd_date",
                schema: "clean",
                table: "su_user_profile");

            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropColumn(
                name: "upd_date",
                schema: "clean",
                table: "su_user_org");

            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "su_user");

            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropColumn(
                name: "upd_date",
                schema: "clean",
                table: "su_profile");

            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "su_organize");

            migrationBuilder.DropColumn(
                name: "cr_by",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.DropColumn(
                name: "cr_date",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.DropColumn(
                name: "upd_by",
                schema: "clean",
                table: "db_employee");

            migrationBuilder.RenameColumn(
                name: "upd_date",
                schema: "clean",
                table: "su_user",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "upd_date",
                schema: "clean",
                table: "su_organize",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "upd_date",
                schema: "clean",
                table: "db_employee",
                newName: "updated_at");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "clean",
                table: "su_user_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "joined_at",
                schema: "clean",
                table: "su_user_org",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "clean",
                table: "su_user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "clean",
                table: "su_profile",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "clean",
                table: "su_organize",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                schema: "clean",
                table: "db_employee",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }
    }
}
