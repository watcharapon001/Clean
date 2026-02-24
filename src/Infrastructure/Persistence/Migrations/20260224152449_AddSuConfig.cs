using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSuConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "su_config",
                schema: "clean",
                columns: table => new
                {
                    config_key = table.Column<string>(type: "text", nullable: false),
                    config_value = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    data_type = table.Column<string>(type: "text", nullable: false),
                    cr_by = table.Column<string>(type: "text", nullable: true),
                    cr_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    upd_by = table.Column<string>(type: "text", nullable: true),
                    upd_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_su_config", x => x.config_key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "su_config",
                schema: "clean");
        }
    }
}
