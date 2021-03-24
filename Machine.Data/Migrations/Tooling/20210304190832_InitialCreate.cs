using Microsoft.EntityFrameworkCore.Migrations;

namespace Machine.Data.Migrations.Tooling
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Toolings",
                columns: table => new
                {
                    ToolingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Machine = table.Column<string>(type: "TEXT", nullable: true),
                    Tools = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toolings", x => x.ToolingID);
                });

            migrationBuilder.CreateTable(
                name: "ToolingUnits",
                columns: table => new
                {
                    ToolingUnitID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToolHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolName = table.Column<string>(type: "TEXT", nullable: true),
                    ToolingID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolingUnits", x => x.ToolingUnitID);
                    table.ForeignKey(
                        name: "FK_ToolingUnits_Toolings_ToolingID",
                        column: x => x.ToolingID,
                        principalTable: "Toolings",
                        principalColumn: "ToolingID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToolingUnits_ToolingID",
                table: "ToolingUnits",
                column: "ToolingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolingUnits");

            migrationBuilder.DropTable(
                name: "Toolings");
        }
    }
}
