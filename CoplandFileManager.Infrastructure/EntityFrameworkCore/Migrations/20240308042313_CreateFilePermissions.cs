using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoplandFileManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateFilePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Files_ObjectId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Files",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserFilePermissions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Permission = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFilePermissions", x => new { x.UserId, x.FileId });
                    table.ForeignKey(
                        name: "FK_UserFilePermissions_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFilePermissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ObjectRoute",
                table: "Files",
                column: "ObjectRoute",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFilePermissions_FileId",
                table: "UserFilePermissions",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFilePermissions");

            migrationBuilder.DropIndex(
                name: "IX_Files_ObjectRoute",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Files");

            migrationBuilder.AddColumn<string>(
                name: "ObjectId",
                table: "Files",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Files_ObjectId",
                table: "Files",
                column: "ObjectId",
                unique: true);
        }
    }
}
