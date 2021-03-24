using Microsoft.EntityFrameworkCore.Migrations;

namespace Machine.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    B = table.Column<byte>(type: "INTEGER", nullable: false),
                    G = table.Column<byte>(type: "INTEGER", nullable: false),
                    R = table.Column<byte>(type: "INTEGER", nullable: false),
                    A = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    LinkID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.LinkID);
                });

            migrationBuilder.CreateTable(
                name: "Matrices",
                columns: table => new
                {
                    MatrixID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    M11 = table.Column<double>(type: "REAL", nullable: false),
                    M12 = table.Column<double>(type: "REAL", nullable: false),
                    M13 = table.Column<double>(type: "REAL", nullable: false),
                    M21 = table.Column<double>(type: "REAL", nullable: false),
                    M23 = table.Column<double>(type: "REAL", nullable: false),
                    M31 = table.Column<double>(type: "REAL", nullable: false),
                    M32 = table.Column<double>(type: "REAL", nullable: false),
                    M33 = table.Column<double>(type: "REAL", nullable: false),
                    OffsetX = table.Column<double>(type: "REAL", nullable: false),
                    OffsetY = table.Column<double>(type: "REAL", nullable: false),
                    M22 = table.Column<double>(type: "REAL", nullable: false),
                    OffsetZ = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matrices", x => x.MatrixID);
                });

            migrationBuilder.CreateTable(
                name: "Vectors",
                columns: table => new
                {
                    VectorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Z = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vectors", x => x.VectorID);
                });

            migrationBuilder.CreateTable(
                name: "LinearLink",
                columns: table => new
                {
                    LinkID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Min = table.Column<double>(type: "REAL", nullable: false),
                    Max = table.Column<double>(type: "REAL", nullable: false),
                    Pos = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinearLink", x => x.LinkID);
                    table.ForeignKey(
                        name: "FK_LinearLink_Links_LinkID",
                        column: x => x.LinkID,
                        principalTable: "Links",
                        principalColumn: "LinkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PneumaticLink",
                columns: table => new
                {
                    LinkID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OffPos = table.Column<double>(type: "REAL", nullable: false),
                    OnPos = table.Column<double>(type: "REAL", nullable: false),
                    TOff = table.Column<double>(type: "REAL", nullable: false),
                    TOn = table.Column<double>(type: "REAL", nullable: false),
                    ToolActivator = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PneumaticLink", x => x.LinkID);
                    table.ForeignKey(
                        name: "FK_PneumaticLink_Links_LinkID",
                        column: x => x.LinkID,
                        principalTable: "Links",
                        principalColumn: "LinkID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineElements",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ModelFile = table.Column<string>(type: "TEXT", nullable: true),
                    ColorID = table.Column<int>(type: "INTEGER", nullable: true),
                    TransformationMatrixID = table.Column<int>(type: "INTEGER", nullable: true),
                    LinkToParentLinkID = table.Column<int>(type: "INTEGER", nullable: true),
                    MachineElementID1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineElements", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_MachineElements_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineElements_Links_LinkToParentLinkID",
                        column: x => x.LinkToParentLinkID,
                        principalTable: "Links",
                        principalColumn: "LinkID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineElements_MachineElements_MachineElementID1",
                        column: x => x.MachineElementID1,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MachineElements_Matrices_TransformationMatrixID",
                        column: x => x.TransformationMatrixID,
                        principalTable: "Matrices",
                        principalColumn: "MatrixID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ColliderElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Radius = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColliderElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_ColliderElement_MachineElements_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InjectorElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InserterId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionVectorID = table.Column<int>(type: "INTEGER", nullable: true),
                    DirectionVectorID = table.Column<int>(type: "INTEGER", nullable: true),
                    InserterColorColorID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InjectorElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_InjectorElement_Colors_InserterColorColorID",
                        column: x => x.InserterColorColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InjectorElement_MachineElements_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InjectorElement_Vectors_DirectionVectorID",
                        column: x => x.DirectionVectorID,
                        principalTable: "Vectors",
                        principalColumn: "VectorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InjectorElement_Vectors_PositionVectorID",
                        column: x => x.PositionVectorID,
                        principalTable: "Vectors",
                        principalColumn: "VectorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PanelHolderElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PanelHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    PanelHolderName = table.Column<string>(type: "TEXT", nullable: true),
                    PositionVectorID = table.Column<int>(type: "INTEGER", nullable: true),
                    Corner = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PanelHolderElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_PanelHolderElement_MachineElements_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PanelHolderElement_Vectors_PositionVectorID",
                        column: x => x.PositionVectorID,
                        principalTable: "Vectors",
                        principalColumn: "VectorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RootElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssemblyName = table.Column<string>(type: "TEXT", nullable: true),
                    RootType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_RootElement_MachineElements_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToolholderElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ToolHolderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolHolderType = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionVectorID = table.Column<int>(type: "INTEGER", nullable: true),
                    DirectionVectorID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolholderElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_ToolholderElement_MachineElements_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "MachineElements",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolholderElement_Vectors_DirectionVectorID",
                        column: x => x.DirectionVectorID,
                        principalTable: "Vectors",
                        principalColumn: "VectorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToolholderElement_Vectors_PositionVectorID",
                        column: x => x.PositionVectorID,
                        principalTable: "Vectors",
                        principalColumn: "VectorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    PointID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false),
                    Z = table.Column<double>(type: "REAL", nullable: false),
                    ColliderElementMachineElementID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.PointID);
                    table.ForeignKey(
                        name: "FK_Points_ColliderElement_ColliderElementMachineElementID",
                        column: x => x.ColliderElementMachineElementID,
                        principalTable: "ColliderElement",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InserterElement",
                columns: table => new
                {
                    MachineElementID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Diameter = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    LoaderLinkId = table.Column<int>(type: "INTEGER", nullable: false),
                    DischargerLinkId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InserterElement", x => x.MachineElementID);
                    table.ForeignKey(
                        name: "FK_InserterElement_InjectorElement_MachineElementID",
                        column: x => x.MachineElementID,
                        principalTable: "InjectorElement",
                        principalColumn: "MachineElementID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InjectorElement_DirectionVectorID",
                table: "InjectorElement",
                column: "DirectionVectorID");

            migrationBuilder.CreateIndex(
                name: "IX_InjectorElement_InserterColorColorID",
                table: "InjectorElement",
                column: "InserterColorColorID");

            migrationBuilder.CreateIndex(
                name: "IX_InjectorElement_PositionVectorID",
                table: "InjectorElement",
                column: "PositionVectorID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineElements_ColorID",
                table: "MachineElements",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineElements_LinkToParentLinkID",
                table: "MachineElements",
                column: "LinkToParentLinkID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineElements_MachineElementID1",
                table: "MachineElements",
                column: "MachineElementID1");

            migrationBuilder.CreateIndex(
                name: "IX_MachineElements_TransformationMatrixID",
                table: "MachineElements",
                column: "TransformationMatrixID");

            migrationBuilder.CreateIndex(
                name: "IX_PanelHolderElement_PositionVectorID",
                table: "PanelHolderElement",
                column: "PositionVectorID");

            migrationBuilder.CreateIndex(
                name: "IX_Points_ColliderElementMachineElementID",
                table: "Points",
                column: "ColliderElementMachineElementID");

            migrationBuilder.CreateIndex(
                name: "IX_ToolholderElement_DirectionVectorID",
                table: "ToolholderElement",
                column: "DirectionVectorID");

            migrationBuilder.CreateIndex(
                name: "IX_ToolholderElement_PositionVectorID",
                table: "ToolholderElement",
                column: "PositionVectorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InserterElement");

            migrationBuilder.DropTable(
                name: "LinearLink");

            migrationBuilder.DropTable(
                name: "PanelHolderElement");

            migrationBuilder.DropTable(
                name: "PneumaticLink");

            migrationBuilder.DropTable(
                name: "Points");

            migrationBuilder.DropTable(
                name: "RootElement");

            migrationBuilder.DropTable(
                name: "ToolholderElement");

            migrationBuilder.DropTable(
                name: "InjectorElement");

            migrationBuilder.DropTable(
                name: "ColliderElement");

            migrationBuilder.DropTable(
                name: "Vectors");

            migrationBuilder.DropTable(
                name: "MachineElements");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "Matrices");
        }
    }
}
