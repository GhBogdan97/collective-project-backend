using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseAccess.Data.Migrations
{
    public partial class AddNameToInternship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Internships",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Internships");
        }
    }
}
