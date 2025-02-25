using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusApp.Migrations
{
    /// <inheritdoc />
    public partial class IsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BusRoutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Buses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "BusRoutes",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BusRoutes",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BusRoutes",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BusRoutes",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Buses",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Buses",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2326), new byte[] { 17, 132, 67, 69, 109, 37, 147, 94, 50, 173, 115, 251, 106, 247, 23, 156, 68, 229, 95, 79, 211, 200, 42, 74, 235, 1, 108, 157, 42, 115, 12, 81, 4, 76, 144, 53, 54, 38, 223, 5, 53, 208, 226, 213, 122, 242, 117, 90, 114, 14, 144, 188, 117, 33, 42, 232, 78, 181, 26, 18, 90, 28, 54, 64 }, new byte[] { 254, 164, 51, 53, 22, 253, 88, 249, 254, 40, 96, 31, 241, 34, 188, 193, 127, 60, 92, 221, 25, 41, 217, 73, 116, 68, 134, 203, 17, 42, 61, 103, 23, 250, 214, 153, 195, 49, 72, 185, 253, 28, 54, 28, 14, 169, 37, 166, 57, 134, 66, 249, 154, 223, 249, 108, 215, 190, 44, 84, 84, 84, 88, 70, 7, 215, 33, 57, 243, 247, 165, 123, 170, 95, 71, 88, 172, 184, 181, 43, 95, 55, 166, 144, 104, 59, 111, 164, 144, 13, 248, 182, 200, 56, 255, 10, 97, 178, 202, 246, 115, 33, 79, 56, 78, 242, 123, 163, 83, 200, 37, 197, 111, 91, 115, 88, 131, 17, 128, 183, 64, 104, 27, 146, 56, 9, 216, 5 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "anuraj@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2385), new byte[] { 172, 236, 228, 8, 233, 111, 200, 164, 103, 177, 238, 0, 16, 16, 118, 208, 140, 99, 219, 41, 178, 214, 46, 187, 41, 27, 117, 76, 84, 148, 2, 15, 161, 206, 206, 221, 231, 61, 255, 74, 218, 237, 250, 126, 68, 62, 208, 192, 172, 181, 47, 229, 70, 115, 68, 61, 29, 144, 142, 182, 118, 66, 183, 188 }, new byte[] { 37, 55, 7, 242, 161, 185, 186, 194, 148, 160, 91, 166, 193, 184, 218, 89, 20, 162, 54, 60, 245, 251, 206, 91, 214, 9, 112, 177, 184, 32, 26, 119, 237, 177, 184, 34, 117, 100, 3, 21, 254, 90, 178, 210, 196, 35, 138, 174, 122, 61, 163, 87, 162, 67, 198, 240, 172, 52, 60, 216, 210, 251, 68, 111, 187, 32, 169, 75, 213, 80, 147, 247, 129, 15, 144, 49, 14, 152, 13, 43, 170, 21, 40, 216, 193, 132, 178, 156, 211, 137, 23, 243, 217, 208, 248, 128, 183, 29, 204, 119, 88, 169, 67, 245, 26, 242, 175, 65, 152, 195, 136, 211, 181, 212, 99, 223, 91, 211, 188, 55, 21, 93, 24, 80, 212, 220, 239, 238 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "smartbus@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2382), new byte[] { 13, 100, 239, 85, 80, 178, 117, 166, 123, 26, 141, 13, 89, 211, 203, 15, 73, 80, 112, 96, 161, 128, 82, 144, 115, 127, 117, 165, 49, 80, 250, 140, 43, 89, 184, 108, 249, 174, 62, 126, 166, 116, 20, 229, 243, 247, 170, 107, 195, 183, 220, 255, 80, 31, 197, 86, 169, 116, 163, 96, 170, 208, 218, 74 }, new byte[] { 183, 179, 54, 138, 240, 3, 199, 220, 117, 247, 142, 4, 120, 185, 233, 30, 67, 94, 1, 215, 13, 143, 62, 198, 90, 62, 69, 163, 60, 13, 120, 121, 248, 134, 186, 148, 44, 2, 163, 203, 80, 54, 71, 20, 240, 142, 199, 98, 24, 134, 62, 101, 121, 153, 93, 65, 12, 200, 42, 131, 202, 116, 37, 36, 222, 165, 123, 61, 233, 28, 40, 221, 115, 148, 207, 240, 253, 16, 251, 122, 36, 68, 37, 230, 224, 110, 154, 47, 9, 132, 173, 118, 78, 180, 172, 131, 3, 35, 68, 92, 76, 238, 130, 8, 42, 66, 87, 30, 136, 136, 90, 132, 175, 113, 177, 77, 116, 58, 180, 22, 181, 90, 96, 98, 5, 172, 168, 16 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BusRoutes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Buses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8510), new byte[] { 117, 117, 207, 18, 139, 6, 77, 167, 209, 134, 99, 200, 88, 3, 115, 131, 148, 112, 156, 84, 6, 112, 62, 142, 30, 226, 24, 19, 239, 233, 184, 76, 24, 234, 158, 194, 76, 147, 231, 99, 32, 14, 225, 190, 88, 140, 177, 181, 10, 203, 253, 222, 4, 240, 79, 190, 119, 176, 196, 98, 30, 73, 46, 208 }, new byte[] { 238, 9, 148, 94, 17, 247, 95, 110, 156, 188, 236, 203, 174, 168, 197, 165, 235, 252, 156, 198, 169, 73, 127, 20, 213, 100, 156, 64, 169, 159, 187, 81, 134, 51, 106, 113, 227, 108, 173, 102, 127, 233, 51, 139, 227, 45, 113, 116, 141, 203, 201, 211, 55, 246, 140, 160, 132, 186, 2, 90, 219, 155, 132, 254, 140, 4, 243, 245, 209, 244, 221, 239, 111, 102, 4, 12, 75, 16, 228, 37, 106, 113, 143, 50, 241, 107, 53, 179, 101, 128, 202, 143, 140, 28, 14, 37, 102, 19, 68, 27, 68, 65, 165, 243, 55, 57, 131, 233, 32, 119, 67, 185, 8, 176, 71, 129, 89, 206, 191, 177, 52, 93, 139, 5, 207, 158, 238, 32 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "anuraj@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8514), new byte[] { 94, 41, 119, 167, 129, 27, 222, 69, 218, 199, 50, 178, 182, 176, 25, 16, 225, 196, 47, 165, 37, 115, 118, 88, 133, 181, 49, 35, 103, 110, 218, 149, 10, 55, 104, 142, 230, 144, 178, 85, 155, 3, 225, 33, 84, 82, 167, 219, 106, 23, 144, 190, 94, 121, 49, 87, 72, 100, 104, 172, 93, 168, 203, 0 }, new byte[] { 175, 12, 246, 63, 97, 54, 194, 23, 227, 198, 30, 115, 5, 73, 250, 196, 92, 161, 3, 31, 90, 238, 177, 69, 230, 168, 178, 17, 63, 247, 238, 48, 124, 43, 182, 143, 1, 136, 61, 146, 119, 54, 11, 96, 182, 141, 10, 153, 50, 64, 218, 53, 171, 29, 22, 154, 84, 6, 112, 28, 147, 61, 188, 224, 200, 142, 202, 18, 164, 242, 7, 134, 133, 50, 244, 34, 98, 139, 140, 40, 31, 203, 218, 146, 213, 54, 204, 75, 32, 206, 133, 210, 172, 63, 137, 82, 47, 243, 44, 190, 231, 42, 64, 245, 129, 213, 232, 235, 170, 238, 250, 22, 121, 55, 175, 73, 226, 92, 29, 69, 74, 122, 135, 249, 0, 214, 205, 62 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "smartbus@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8512), new byte[] { 90, 19, 99, 40, 23, 255, 135, 159, 14, 49, 206, 240, 2, 68, 137, 56, 178, 20, 88, 252, 82, 46, 237, 111, 122, 245, 60, 157, 165, 46, 89, 9, 159, 177, 116, 72, 161, 100, 12, 130, 194, 83, 212, 186, 201, 41, 63, 106, 57, 0, 79, 195, 119, 192, 18, 112, 245, 195, 221, 232, 149, 8, 16, 123 }, new byte[] { 123, 133, 78, 220, 192, 134, 0, 49, 9, 54, 176, 39, 26, 224, 165, 31, 201, 201, 61, 186, 174, 217, 243, 8, 98, 223, 31, 216, 14, 241, 135, 167, 26, 43, 39, 41, 237, 222, 219, 164, 166, 224, 126, 172, 177, 217, 65, 103, 173, 230, 248, 150, 202, 151, 1, 227, 5, 203, 9, 111, 8, 227, 139, 221, 227, 104, 93, 230, 176, 86, 116, 215, 72, 34, 244, 174, 152, 173, 120, 42, 216, 6, 224, 251, 121, 212, 35, 91, 186, 146, 120, 112, 251, 0, 56, 240, 121, 19, 112, 229, 202, 44, 31, 162, 242, 36, 244, 220, 66, 164, 52, 48, 220, 25, 106, 99, 124, 64, 111, 70, 7, 183, 200, 145, 32, 149, 14, 221 } });
        }
    }
}
