using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBangAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixOccupationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ocuppation",
                table: "Characters",
                newName: "Occupation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Occupation",
                table: "Characters",
                newName: "Ocuppation");
        }
    }
}
