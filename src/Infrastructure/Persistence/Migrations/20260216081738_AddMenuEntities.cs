using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "su_menu",
                schema: "clean",
                columns: table => new
                {
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_code = table.Column<string>(type: "text", nullable: false),
                    menu_name = table.Column<string>(type: "text", nullable: false),
                    route = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    sequence = table.Column<int>(type: "integer", nullable: false),
                    parent_menu_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    cr_by = table.Column<string>(type: "text", nullable: true),
                    cr_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    upd_by = table.Column<string>(type: "text", nullable: true),
                    upd_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_menu", x => x.menu_id);
                    table.ForeignKey(
                        name: "fk_su_menu_su_menu_parent_menu_temp_id",
                        column: x => x.parent_menu_id,
                        principalSchema: "clean",
                        principalTable: "su_menu",
                        principalColumn: "menu_id");
                });

            migrationBuilder.CreateTable(
                name: "su_profile_menu",
                schema: "clean",
                columns: table => new
                {
                    profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    menu_id = table.Column<Guid>(type: "uuid", nullable: false),
                    can_view = table.Column<bool>(type: "boolean", nullable: false),
                    can_create = table.Column<bool>(type: "boolean", nullable: false),
                    can_edit = table.Column<bool>(type: "boolean", nullable: false),
                    can_delete = table.Column<bool>(type: "boolean", nullable: false),
                    cr_by = table.Column<string>(type: "text", nullable: true),
                    cr_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    upd_by = table.Column<string>(type: "text", nullable: true),
                    upd_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_su_profile_menu", x => new { x.profile_id, x.menu_id });
                    table.ForeignKey(
                        name: "fk_su_profile_menu_su_menu_menu_temp_id1",
                        column: x => x.menu_id,
                        principalSchema: "clean",
                        principalTable: "su_menu",
                        principalColumn: "menu_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_su_profile_menu_su_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "clean",
                        principalTable: "su_profile",
                        principalColumn: "profile_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_su_menu_parent_menu_id",
                schema: "clean",
                table: "su_menu",
                column: "parent_menu_id");

            migrationBuilder.CreateIndex(
                name: "IX_su_profile_menu_menu_id",
                schema: "clean",
                table: "su_profile_menu",
                column: "menu_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "su_profile_menu",
                schema: "clean");

            migrationBuilder.DropTable(
                name: "su_menu",
                schema: "clean");
        }
    }
}
