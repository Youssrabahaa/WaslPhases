using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phase_1.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Conversations",
                columns: new[] { "Id", "CreatedAt", "IsClosed", "LastMessageAt", "MatchId" },
                values: new object[] { 1, new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), false, null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Conversations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
