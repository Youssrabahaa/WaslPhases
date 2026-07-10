using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phase_1.Migrations
{
    public partial class AddMatchSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 2);

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "CaseId", "CreatedAt", "DecidedAt", "EstimatedSessionsCount", "Message", "ProposedPrice", "Status", "StudentUserId" },
                values: new object[] { 1, 1, new DateTime(2026, 4, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), 3, "أقترح البدء بتقييم داخل العيادة ثم جلستين علاجيتين حسب نتيجة الفحص.", 220.00m, 2, 3 });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "AcceptedAt", "CaseId", "CompletedAt", "CreatedAt", "OfferId", "PatientUserId", "PhoneContactOnly", "Status", "StudentUserId" },
                values: new object[] { 1, new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), 1, null, new DateTime(2026, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1, true, 1, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Matches",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 1,
                column: "Status",
                value: 1);
        }
    }
}
