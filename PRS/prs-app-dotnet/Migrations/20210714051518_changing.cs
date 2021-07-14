using Microsoft.EntityFrameworkCore.Migrations;

namespace prs_app_dotnet.Migrations
{
    public partial class changing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataNeeded",
                table: "Requests",
                newName: "DateNeeded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateNeeded",
                table: "Requests",
                newName: "DataNeeded");
        }
    }
}
