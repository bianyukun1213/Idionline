using Microsoft.EntityFrameworkCore.Migrations;

namespace Idionline.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idioms",
                columns: table => new
                {
                    IdiomName = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Definitions = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    LastEditor = table.Column<string>(nullable: true),
                    UpdateTimeUT = table.Column<long>(nullable: false),
                    Index = table.Column<char>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idioms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaunchInf",
                columns: table => new
                {
                    Text = table.Column<string>(nullable: true),
                    DailyIdiomName = table.Column<string>(nullable: true),
                    DailyIdiomId = table.Column<int>(nullable: false),
                    DateUT = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaunchInf", x => x.DateUT);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Idioms");

            migrationBuilder.DropTable(
                name: "LaunchInf");
        }
    }
}
