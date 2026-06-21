using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace phase_1.Migrations
{
    /// <inheritdoc />
    public partial class SeedReferenceAndDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Home Visit" },
                    { 2, true, "In-Clinic Session" },
                    { 3, true, "Online Follow-Up" }
                });

            migrationBuilder.InsertData(
                table: "TreatmentCategories",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Dental care cases for children, including preventive treatment and early interventions.", true, "Pediatric Dentistry" },
                    { 2, "Cases involving fillings, simple restorations, and treatment plans to repair tooth structure.", true, "Restorative Dentistry" },
                    { 3, "Cases that may require extractions or minor oral surgical procedures under supervision.", true, "Oral Surgery" }
                });

            migrationBuilder.InsertData(
                table: "Universities",
                columns: new[] { "Id", "CreatedAt", "Governorate", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cairo", "Cairo University" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Alexandria", "Alexandria University" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "LastLoginAt", "Phone", "Role", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc), "mona.ahmed@example.com", "Mona Ahmed Abdelrahman", new DateTime(2026, 4, 10, 18, 30, 0, 0, DateTimeKind.Utc), "01010000001", 1, 1 },
                    { 2, new DateTime(2026, 1, 14, 11, 15, 0, 0, DateTimeKind.Utc), "youssef.mohamed@example.com", "Youssef Mohamed Elsayed", new DateTime(2026, 4, 12, 14, 0, 0, 0, DateTimeKind.Utc), "01010000002", 1, 1 },
                    { 3, new DateTime(2026, 1, 18, 8, 45, 0, 0, DateTimeKind.Utc), "ahmed.khaled@example.com", "Ahmed Khaled Mansour", new DateTime(2026, 4, 15, 10, 20, 0, 0, DateTimeKind.Utc), "01010000003", 2, 1 },
                    { 4, new DateTime(2026, 1, 20, 13, 0, 0, 0, DateTimeKind.Utc), "salma.tarek@example.com", "Salma Tarek Hassan", new DateTime(2026, 4, 16, 9, 10, 0, 0, DateTimeKind.Utc), "01010000004", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Cases",
                columns: new[] { "Id", "Area", "City", "ClosedAt", "CreatedAt", "Description", "EstimatedPriceMax", "EstimatedPriceMin", "Governorate", "NeedsSupervisorApproval", "PatientUserId", "ServiceTypeId", "Status", "Title", "TreatmentCategoryId", "Urgency" },
                values: new object[,]
                {
                    { 1, "Seventh District", "Nasr City", null, new DateTime(2026, 4, 1, 16, 0, 0, 0, DateTimeKind.Utc), "The family is looking for a supervised dental student to help with examination, preventive guidance, and a treatment plan for a 6-year-old child.", 250.00m, 180.00m, "Cairo", true, 1, 1, 1, "Follow-up for a child with multiple dental caries", 1, 2 },
                    { 2, "Zahraa El Maadi", "Maadi", null, new DateTime(2026, 4, 3, 12, 30, 0, 0, DateTimeKind.Utc), "The patient needs an in-clinic supervised student for assessment and restoration planning for painful posterior teeth.", 320.00m, 220.00m, "Cairo", true, 2, 2, 1, "Restorative dental treatment for damaged molars", 2, 3 }
                });

            migrationBuilder.InsertData(
                table: "Faculties",
                columns: new[] { "Id", "CreatedAt", "Location", "Name", "UniversityId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Giza", "Faculty of Dentistry", 1, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "El Azareeta", "Faculty of Dentistry", 2, null }
                });

            migrationBuilder.InsertData(
                table: "PatientProfiles",
                columns: new[] { "UserId", "BirthDate", "Gender", "Notes" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Female", "Young child who needs regular follow-up for articulation and communication skills." },
                    { 2, new DateTime(1988, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "Needs speech rehabilitation sessions after a stroke with family support." }
                });

            migrationBuilder.InsertData(
                table: "StudentProfiles",
                columns: new[] { "UserId", "AcademicYear", "ClinicName", "CompletedCasesCount", "FacultyId", "IsVerified", "RequiredCasesCount", "StudentCode", "SupervisorName", "VerifiedAt" },
                values: new object[,]
                {
                    { 3, 4, "Kasr Al-Ainy Teaching Dental Clinic", 5, 1, true, 8, "CAI-DEN-2401", "Dr. Hala Samir", new DateTime(2026, 2, 1, 12, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 3, "Alexandria University Dental Hospital", 3, 2, true, 6, "ALX-DEN-1732", "Dr. Naglaa Adel", new DateTime(2026, 2, 5, 10, 30, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cases",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PatientProfiles",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PatientProfiles",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StudentProfiles",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StudentProfiles",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Faculties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Universities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Universities",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
