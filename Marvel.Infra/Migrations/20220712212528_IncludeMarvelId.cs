using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marvel.Infra.Migrations
{
    public partial class IncludeMarvelId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarvelId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarvelId",
                table: "Characters");
        }
    }
}
