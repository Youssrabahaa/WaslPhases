using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phase_1.Migrations
{
    public partial class Notfications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_HandledByAdminId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReporterId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_TargetType_TargetId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "TargetType",
                table: "Reports",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "Reports",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "ReporterId",
                table: "Reports",
                newName: "ReporterUserId");

            migrationBuilder.RenameColumn(
                name: "ReportedId",
                table: "Reports",
                newName: "ReportedUserId");

            migrationBuilder.RenameColumn(
                name: "HandledByAdminId",
                table: "Reports",
                newName: "ApplicationUserId2");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                newName: "IX_Reports_ReporterUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportedId",
                table: "Reports",
                newName: "IX_Reports_ReportedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_HandledByAdminId",
                table: "Reports",
                newName: "IX_Reports_ApplicationUserId2");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId1",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Reports",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApplicationUserId",
                table: "Reports",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ApplicationUserId1",
                table: "Reports",
                column: "ApplicationUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SessionId",
                table: "Reports",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Sessions_SessionId",
                table: "Reports",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ApplicationUserId",
                table: "Reports",
                column: "ApplicationUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ApplicationUserId1",
                table: "Reports",
                column: "ApplicationUserId1",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ApplicationUserId2",
                table: "Reports",
                column: "ApplicationUserId2",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedUserId",
                table: "Reports",
                column: "ReportedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReporterUserId",
                table: "Reports",
                column: "ReporterUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Sessions_SessionId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ApplicationUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ApplicationUserId1",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ApplicationUserId2",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReportedUserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_ReporterUserId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ApplicationUserId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ApplicationUserId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SessionId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Reports",
                newName: "TargetType");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "Reports",
                newName: "TargetId");

            migrationBuilder.RenameColumn(
                name: "ReporterUserId",
                table: "Reports",
                newName: "ReporterId");

            migrationBuilder.RenameColumn(
                name: "ReportedUserId",
                table: "Reports",
                newName: "ReportedId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId2",
                table: "Reports",
                newName: "HandledByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReporterUserId",
                table: "Reports",
                newName: "IX_Reports_ReporterId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportedUserId",
                table: "Reports",
                newName: "IX_Reports_ReportedId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ApplicationUserId2",
                table: "Reports",
                newName: "IX_Reports_HandledByAdminId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Reports",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TargetType_TargetId",
                table: "Reports",
                columns: new[] { "TargetType", "TargetId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_HandledByAdminId",
                table: "Reports",
                column: "HandledByAdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReportedId",
                table: "Reports",
                column: "ReportedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_ReporterId",
                table: "Reports",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
