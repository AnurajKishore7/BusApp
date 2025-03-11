﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusApp.Migrations
{
    /// <inheritdoc />
    public partial class AddContactToBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "TicketPassengers");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Bookings",
                type: "nvarchar(15)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 9, 23, 19, 51, 798, DateTimeKind.Local).AddTicks(5452), new byte[] { 10, 229, 124, 181, 142, 10, 92, 15, 95, 233, 114, 146, 142, 28, 42, 239, 182, 203, 84, 231, 29, 193, 111, 120, 65, 163, 22, 238, 84, 59, 249, 215, 25, 41, 128, 248, 214, 9, 195, 184, 206, 124, 63, 6, 218, 70, 214, 185, 179, 109, 98, 210, 48, 244, 170, 184, 57, 113, 103, 127, 0, 230, 197, 129 }, new byte[] { 22, 239, 129, 51, 225, 78, 143, 17, 198, 97, 221, 20, 24, 161, 132, 235, 83, 156, 70, 64, 163, 250, 7, 26, 24, 211, 184, 37, 24, 250, 20, 113, 46, 19, 173, 86, 123, 147, 197, 15, 78, 128, 210, 134, 4, 152, 226, 219, 255, 147, 122, 26, 21, 172, 148, 160, 211, 95, 210, 17, 28, 97, 30, 49, 144, 123, 36, 66, 80, 20, 45, 97, 77, 109, 234, 228, 114, 242, 178, 189, 55, 243, 118, 72, 58, 114, 78, 119, 156, 214, 244, 199, 125, 216, 174, 141, 108, 12, 218, 54, 202, 77, 212, 26, 78, 153, 111, 117, 176, 124, 0, 56, 147, 103, 250, 48, 28, 25, 33, 164, 89, 13, 176, 135, 23, 223, 155, 182 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "anuraj@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 9, 23, 19, 51, 798, DateTimeKind.Local).AddTicks(5455), new byte[] { 102, 46, 31, 63, 48, 150, 45, 95, 246, 198, 241, 234, 143, 53, 246, 121, 108, 57, 28, 182, 17, 23, 20, 6, 239, 108, 235, 67, 222, 68, 200, 127, 84, 176, 53, 231, 167, 175, 117, 49, 158, 7, 44, 20, 114, 208, 88, 228, 76, 54, 88, 226, 96, 210, 89, 119, 37, 101, 49, 84, 198, 242, 73, 117 }, new byte[] { 105, 73, 110, 58, 179, 198, 241, 81, 94, 116, 233, 177, 61, 30, 137, 116, 122, 126, 125, 102, 244, 84, 142, 168, 35, 229, 110, 127, 63, 63, 125, 208, 122, 5, 95, 45, 117, 158, 61, 190, 32, 32, 232, 79, 130, 28, 166, 119, 11, 253, 233, 120, 202, 4, 24, 178, 115, 94, 59, 193, 205, 116, 1, 100, 242, 208, 152, 18, 179, 52, 36, 72, 133, 112, 214, 131, 203, 18, 172, 70, 254, 176, 33, 72, 185, 28, 37, 29, 86, 171, 93, 242, 206, 171, 10, 135, 121, 55, 94, 195, 156, 6, 88, 145, 39, 73, 45, 10, 236, 14, 227, 28, 241, 90, 39, 199, 212, 182, 73, 82, 119, 151, 196, 131, 210, 133, 54, 122 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "smartbus@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 9, 23, 19, 51, 798, DateTimeKind.Local).AddTicks(5456), new byte[] { 148, 40, 201, 171, 233, 18, 79, 241, 42, 113, 87, 153, 109, 155, 197, 250, 119, 119, 153, 195, 169, 79, 89, 232, 131, 177, 231, 66, 188, 247, 60, 82, 147, 8, 216, 126, 128, 77, 3, 52, 195, 189, 7, 90, 39, 150, 35, 80, 235, 171, 44, 248, 95, 214, 221, 192, 85, 93, 51, 20, 8, 65, 71, 198 }, new byte[] { 48, 104, 90, 133, 6, 65, 211, 56, 103, 130, 199, 117, 231, 56, 76, 58, 104, 97, 224, 30, 90, 137, 29, 207, 52, 241, 165, 116, 221, 207, 215, 38, 79, 238, 233, 127, 70, 245, 158, 205, 185, 0, 60, 244, 171, 2, 6, 164, 53, 202, 153, 221, 66, 160, 188, 94, 102, 28, 99, 107, 107, 83, 10, 185, 15, 195, 150, 193, 240, 6, 197, 240, 97, 104, 68, 163, 0, 20, 121, 88, 17, 229, 56, 102, 121, 75, 30, 6, 74, 186, 134, 9, 214, 122, 53, 74, 4, 210, 37, 195, 222, 161, 203, 10, 34, 193, 71, 99, 0, 154, 244, 171, 209, 207, 157, 230, 168, 97, 59, 95, 57, 248, 15, 219, 62, 243, 91, 60 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Bookings");

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "TicketPassengers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3084), new byte[] { 116, 88, 87, 25, 97, 134, 104, 24, 17, 3, 51, 58, 104, 67, 124, 92, 240, 128, 81, 64, 69, 159, 129, 206, 224, 144, 52, 45, 186, 35, 142, 18, 171, 73, 169, 70, 89, 50, 32, 117, 96, 182, 178, 250, 129, 69, 68, 81, 38, 129, 127, 48, 27, 76, 209, 209, 108, 31, 194, 158, 100, 17, 37, 42 }, new byte[] { 221, 8, 42, 12, 46, 239, 136, 251, 113, 220, 75, 227, 81, 108, 119, 93, 28, 162, 226, 118, 75, 112, 21, 132, 55, 41, 188, 126, 160, 61, 216, 182, 195, 145, 254, 84, 10, 130, 70, 189, 108, 167, 96, 181, 240, 88, 94, 222, 43, 229, 19, 84, 169, 162, 147, 39, 135, 99, 29, 164, 40, 94, 165, 154, 175, 227, 123, 95, 255, 200, 135, 39, 149, 139, 217, 16, 188, 221, 192, 216, 82, 148, 29, 218, 140, 32, 234, 205, 73, 136, 201, 214, 230, 241, 77, 42, 252, 18, 41, 186, 32, 40, 212, 254, 198, 208, 18, 183, 148, 137, 117, 9, 235, 26, 43, 7, 119, 127, 42, 2, 130, 127, 40, 210, 222, 233, 239, 206 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "anuraj@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3087), new byte[] { 43, 62, 56, 214, 243, 254, 242, 231, 84, 35, 200, 31, 199, 45, 149, 202, 128, 188, 28, 214, 7, 249, 117, 66, 165, 133, 135, 252, 89, 87, 200, 162, 206, 172, 22, 128, 51, 20, 224, 172, 13, 98, 3, 168, 244, 95, 135, 234, 84, 170, 141, 7, 149, 237, 183, 106, 71, 120, 105, 45, 222, 191, 103, 39 }, new byte[] { 15, 94, 156, 47, 13, 224, 22, 113, 38, 44, 57, 114, 23, 57, 194, 184, 120, 56, 174, 193, 103, 35, 51, 140, 29, 83, 90, 171, 2, 46, 128, 192, 75, 32, 225, 87, 155, 74, 218, 211, 97, 136, 206, 113, 61, 152, 186, 245, 171, 199, 103, 249, 207, 160, 141, 145, 214, 207, 211, 64, 29, 122, 206, 32, 80, 96, 105, 168, 89, 139, 175, 91, 33, 214, 25, 239, 47, 207, 155, 198, 1, 174, 40, 48, 209, 183, 181, 230, 144, 123, 210, 160, 40, 42, 158, 168, 55, 176, 248, 236, 160, 16, 210, 56, 252, 95, 128, 21, 90, 150, 152, 15, 209, 59, 115, 233, 190, 241, 252, 9, 217, 181, 121, 41, 63, 223, 183, 150 } });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "smartbus@gmail.com",
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3089), new byte[] { 176, 74, 239, 75, 170, 167, 115, 2, 186, 234, 134, 116, 165, 244, 249, 40, 46, 160, 104, 49, 25, 82, 80, 215, 80, 230, 127, 21, 250, 5, 202, 193, 57, 122, 147, 55, 123, 196, 58, 75, 248, 16, 102, 99, 5, 21, 245, 30, 63, 193, 3, 169, 118, 33, 128, 92, 149, 231, 79, 225, 201, 2, 99, 164 }, new byte[] { 220, 169, 31, 19, 174, 68, 113, 76, 135, 243, 100, 247, 88, 26, 154, 14, 157, 80, 102, 94, 254, 165, 145, 58, 18, 39, 26, 154, 169, 205, 93, 144, 158, 66, 76, 185, 155, 48, 177, 187, 237, 140, 0, 161, 173, 111, 180, 239, 107, 152, 154, 80, 139, 110, 129, 129, 35, 234, 175, 63, 73, 51, 137, 13, 183, 50, 161, 126, 126, 106, 176, 122, 47, 142, 113, 174, 18, 245, 80, 69, 176, 153, 159, 114, 31, 147, 119, 239, 164, 67, 41, 97, 208, 130, 240, 39, 154, 76, 148, 67, 241, 110, 207, 2, 105, 69, 135, 78, 86, 179, 49, 71, 232, 15, 229, 54, 176, 79, 209, 194, 184, 11, 196, 228, 237, 142, 119, 76 } });
        }
    }
}
