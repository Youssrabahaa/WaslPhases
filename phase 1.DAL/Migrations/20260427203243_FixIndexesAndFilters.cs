using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phase_1.Migrations
{
    public partial class FixIndexesAndFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NoShowStrikes_SessionId_UserId",
                table: "NoShowStrikes",
                columns: new[] { "SessionId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NoShowStrikes_SessionId_UserId",
                table: "NoShowStrikes");
        }
    }
}
