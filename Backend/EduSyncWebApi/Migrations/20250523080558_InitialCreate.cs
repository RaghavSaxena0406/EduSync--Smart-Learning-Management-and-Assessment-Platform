using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduSyncWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__UserMode__A9D10534F75F0D55",
                table: "UserModel");

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "AssessmentResult",
                type: "varchar(max)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "AssessmentResult",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UQ__UserMode__A9D10534F75F0D55",
                table: "UserModel",
                column: "Email",
                unique: true);
        }
    }
}
