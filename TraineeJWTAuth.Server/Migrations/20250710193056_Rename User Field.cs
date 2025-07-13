using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeJWTAuth.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NicName",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "NicName");
        }
    }
}
