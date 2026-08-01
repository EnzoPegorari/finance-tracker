using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTracker.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class TranslateDefaultCategoriesToEnglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Food" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Transport" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111103"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Health" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111104"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Housing" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111105"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Education" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111106"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Leisure" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111107"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Salary" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111108"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Other" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111101"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8888), "Alimentação" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111102"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8892), "Transporte" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111103"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8895), "Saúde" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111104"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8897), "Moradia" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111105"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8899), "Educação" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111106"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8908), "Lazer" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111107"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8910), "Salário" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111108"),
                columns: new[] { "CreatedAt", "Name" },
                values: new object[] { new DateTime(2026, 8, 1, 22, 21, 24, 815, DateTimeKind.Utc).AddTicks(8913), "Outros" });
        }
    }
}
