using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc_webapp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REF_NO = table.Column<string>(nullable: true),
                    FLM_SLM = table.Column<string>(nullable: true),
                    MACH_ID = table.Column<string>(nullable: true),
                    SERIAL_NO = table.Column<string>(nullable: true),
                    LOCATION = table.Column<string>(nullable: true),
                    DATE = table.Column<DateTime>(nullable: false),
                    ARRIVAL_TIME = table.Column<DateTime>(nullable: false),
                    PROBLEM_CODE = table.Column<string>(nullable: true),
                    DESCRIPTION = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
