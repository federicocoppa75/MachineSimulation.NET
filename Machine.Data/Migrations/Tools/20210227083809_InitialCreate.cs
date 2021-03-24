using Microsoft.EntityFrameworkCore.Migrations;

namespace Machine.Data.Migrations.Tools
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToolSets",
                columns: table => new
                {
                    ToolSetID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolSets", x => x.ToolSetID);
                });

            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ToolLinkType = table.Column<int>(type: "INTEGER", nullable: false),
                    ConeModelFile = table.Column<string>(type: "TEXT", nullable: true),
                    ToolSetID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_Tools_ToolSets_ToolSetID",
                        column: x => x.ToolSetID,
                        principalTable: "ToolSets",
                        principalColumn: "ToolSetID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CountersinkTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter1 = table.Column<double>(type: "REAL", nullable: false),
                    Length1 = table.Column<double>(type: "REAL", nullable: false),
                    Diameter2 = table.Column<double>(type: "REAL", nullable: false),
                    Length2 = table.Column<double>(type: "REAL", nullable: false),
                    Length3 = table.Column<double>(type: "REAL", nullable: false),
                    UsefulLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountersinkTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_CountersinkTool_Tools_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tools",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiskTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    CuttingRadialThickness = table.Column<double>(type: "REAL", nullable: false),
                    BodyThickness = table.Column<double>(type: "REAL", nullable: false),
                    CuttingThickness = table.Column<double>(type: "REAL", nullable: false),
                    RadialUsefulLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiskTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_DiskTool_Tools_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tools",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointedTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    StraightLength = table.Column<double>(type: "REAL", nullable: false),
                    ConeHeight = table.Column<double>(type: "REAL", nullable: false),
                    UsefulLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointedTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_PointedTool_Tools_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tools",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimpleTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    UsefulLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_SimpleTool_Tools_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tools",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwoSectionTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter1 = table.Column<double>(type: "REAL", nullable: false),
                    Length1 = table.Column<double>(type: "REAL", nullable: false),
                    Diameter2 = table.Column<double>(type: "REAL", nullable: false),
                    Length2 = table.Column<double>(type: "REAL", nullable: false),
                    UsefulLength = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoSectionTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_TwoSectionTool_Tools_ToolID",
                        column: x => x.ToolID,
                        principalTable: "Tools",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiskOnConeTool",
                columns: table => new
                {
                    ToolID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostponemntLength = table.Column<double>(type: "REAL", nullable: false),
                    PostponemntDiameter = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiskOnConeTool", x => x.ToolID);
                    table.ForeignKey(
                        name: "FK_DiskOnConeTool_DiskTool_ToolID",
                        column: x => x.ToolID,
                        principalTable: "DiskTool",
                        principalColumn: "ToolID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tools_ToolSetID",
                table: "Tools",
                column: "ToolSetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountersinkTool");

            migrationBuilder.DropTable(
                name: "DiskOnConeTool");

            migrationBuilder.DropTable(
                name: "PointedTool");

            migrationBuilder.DropTable(
                name: "SimpleTool");

            migrationBuilder.DropTable(
                name: "TwoSectionTool");

            migrationBuilder.DropTable(
                name: "DiskTool");

            migrationBuilder.DropTable(
                name: "Tools");

            migrationBuilder.DropTable(
                name: "ToolSets");
        }
    }
}
