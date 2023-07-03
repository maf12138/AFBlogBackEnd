using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrasturacture.Migrations
{
    public partial class add_partenrelationship_in_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pid",
                table: "T_Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pid",
                table: "T_Categories");
        }
    }
}
