using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBooking.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Rooms",
                newName: "Amenities");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Hotels",
                newName: "Image3");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Hotels");

            migrationBuilder.RenameColumn(
                name: "Amenities",
                table: "Rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Image3",
                table: "Hotels",
                newName: "Image");
        }
    }
}
