using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBooking.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluate_Payments_PaymentID",
                table: "Evaluate");

            migrationBuilder.DropTable(
                name: "Envaluates");

            migrationBuilder.RenameTable(
                name: "Evaluate",
                newName: "Evaluates");

            migrationBuilder.AlterColumn<double>(
                name: "AverageScore",
                table: "Hotels",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentID",
                table: "Evaluates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EvaluateID",
                table: "Evaluates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Evaluates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EvaluateTime",
                table: "Evaluates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Image1",
                table: "Evaluates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Evaluates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Evaluates",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluates",
                table: "Evaluates",
                column: "EvaluateID");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluates_PaymentID",
                table: "Evaluates",
                column: "PaymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluates_Payments_PaymentID",
                table: "Evaluates",
                column: "PaymentID",
                principalTable: "Payments",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluates_Payments_PaymentID",
                table: "Evaluates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluates",
                table: "Evaluates");

            migrationBuilder.DropIndex(
                name: "IX_Evaluates_PaymentID",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "EvaluateID",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "EvaluateTime",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "Image1",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Evaluates");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Evaluates");

            migrationBuilder.RenameTable(
                name: "Evaluates",
                newName: "Evaluate");

            migrationBuilder.AlterColumn<double>(
                name: "AverageScore",
                table: "Hotels",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PaymentID",
                table: "Evaluate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Envaluates",
                columns: table => new
                {
                    EnvaluateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnvaluateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentID = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Envaluates", x => x.EnvaluateID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Envaluates_PaymentID",
                table: "Envaluates",
                column: "PaymentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluate_Payments_PaymentID",
                table: "Evaluate",
                column: "PaymentID",
                principalTable: "Payments",
                principalColumn: "PaymentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
