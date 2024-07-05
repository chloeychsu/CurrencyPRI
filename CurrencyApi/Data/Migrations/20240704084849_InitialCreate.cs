using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_AuditTrail",
                columns: table => new
                {
                    AuditId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RequestContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Request = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAuthenticated = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ModelState = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ResponseContentType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ClientIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HttpStatus = table.Column<int>(type: "int", nullable: false),
                    ReqeustDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseDateUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AuditTrail", x => x.AuditId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedUTC = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Translation",
                columns: table => new
                {
                    TranslationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Translation", x => x.TranslationId);
                    table.ForeignKey(
                        name: "FK_tbl_Translation_tbl_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "tbl_Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Currencies_Code",
                table: "tbl_Currencies",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Translation_CurrencyId",
                table: "tbl_Translation",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_AuditTrail");

            migrationBuilder.DropTable(
                name: "tbl_Translation");

            migrationBuilder.DropTable(
                name: "tbl_Currencies");
        }
    }
}
