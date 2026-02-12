using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "clean");

            migrationBuilder.CreateTable(
                name: "su_organize",
                schema: "clean",
                columns: table => new
                {
                    org_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    org_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_organize", x => x.org_id);
                });

            migrationBuilder.CreateTable(
                name: "su_profile",
                schema: "clean",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    profile_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    profile_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_profile", x => x.profile_id);
                });

            migrationBuilder.CreateTable(
                name: "db_employee",
                schema: "clean",
                columns: table => new
                {
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    last_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_db_employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_db_employee_su_organize_org_id",
                        column: x => x.org_id,
                        principalSchema: "clean",
                        principalTable: "su_organize",
                        principalColumn: "org_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "su_user",
                schema: "clean",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    email_normalized = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    security_stamp = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_locked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    lockout_end_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    last_login_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_su_user_db_employee_employee_id",
                        column: x => x.employee_id,
                        principalSchema: "clean",
                        principalTable: "db_employee",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "su_user_org",
                schema: "clean",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    org_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    joined_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_user_org", x => new { x.user_id, x.org_id });
                    table.ForeignKey(
                        name: "FK_su_user_org_su_organize_org_id",
                        column: x => x.org_id,
                        principalSchema: "clean",
                        principalTable: "su_organize",
                        principalColumn: "org_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_su_user_org_su_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "clean",
                        principalTable: "su_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "su_user_profile",
                schema: "clean",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_user_profile", x => new { x.user_id, x.profile_id });
                    table.ForeignKey(
                        name: "FK_su_user_profile_su_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "clean",
                        principalTable: "su_profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_su_user_profile_su_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "clean",
                        principalTable: "su_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_db_employee_email",
                schema: "clean",
                table: "db_employee",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_db_employee_org_id",
                schema: "clean",
                table: "db_employee",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "IX_db_employee_org_id_employee_code",
                schema: "clean",
                table: "db_employee",
                columns: new[] { "org_id", "employee_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_su_organize_org_code",
                schema: "clean",
                table: "su_organize",
                column: "org_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_su_profile_profile_code",
                schema: "clean",
                table: "su_profile",
                column: "profile_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_su_user_email_normalized",
                schema: "clean",
                table: "su_user",
                column: "email_normalized",
                unique: true,
                filter: "email_normalized IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_su_user_employee_id",
                schema: "clean",
                table: "su_user",
                column: "employee_id",
                unique: true,
                filter: "employee_id IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_su_user_is_active",
                schema: "clean",
                table: "su_user",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_su_user_username",
                schema: "clean",
                table: "su_user",
                column: "username",
                unique: true,
                filter: "username IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_su_user_org_default_per_user",
                schema: "clean",
                table: "su_user_org",
                column: "user_id",
                unique: true,
                filter: "is_default = true");

            migrationBuilder.CreateIndex(
                name: "IX_su_user_org_org_id",
                schema: "clean",
                table: "su_user_org",
                column: "org_id");

            migrationBuilder.CreateIndex(
                name: "IX_su_user_profile_profile_id",
                schema: "clean",
                table: "su_user_profile",
                column: "profile_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "su_user_org",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "su_user_profile",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "su_profile",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "su_user",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "db_employee",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "su_organize",
                schema: "clean");
        }
    }
}
