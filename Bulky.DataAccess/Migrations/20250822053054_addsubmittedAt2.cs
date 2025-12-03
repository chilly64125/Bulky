using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addsubmittedAt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "KindnessPositions");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "EventRegistrations");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "AncestralPositions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "ShoppingCarts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "ProductImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "KindnessPositions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "EventRegistrations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Companies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Categories",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "AncestralPositions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(994));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 914, DateTimeKind.Utc).AddTicks(996));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2529));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2867));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2871));

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(2874));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8485));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8808));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8810));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8813));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8816));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8817));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8819));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8820));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8822));

            migrationBuilder.UpdateData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(8823));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6137));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6463));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6532));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6535));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "SubmittedAt",
                value: new DateTime(2025, 8, 22, 5, 7, 16, 915, DateTimeKind.Utc).AddTicks(6537));
        }
    }
}
