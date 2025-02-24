using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstimatedDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHandicapped = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "TransportOperators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportOperators_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    BusType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buses_TransportOperators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "TransportOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusRouteId = table.Column<int>(type: "int", nullable: false),
                    BusId = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_BusRoutes_BusRouteId",
                        column: x => x.BusRouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMadeAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPassengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeatNo = table.Column<int>(type: "int", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPassengers_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BusRoutes",
                columns: new[] { "Id", "Destination", "Distance", "EstimatedDuration", "Source" },
                values: new object[,]
                {
                    { 1, "Kanyakumari", 750, "12:30", "Chennai" },
                    { 2, "Chennai", 750, "12:30", "Kanyakumari" },
                    { 3, "Bangalore", 350, "06:00", "Chennai" },
                    { 4, "Chennai", 350, "06:00", "Bangalore" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "CreatedAt", "IsApproved", "IsDeleted", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { "admin@gmail.com", new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8510), true, false, "Super Admin", new byte[] { 117, 117, 207, 18, 139, 6, 77, 167, 209, 134, 99, 200, 88, 3, 115, 131, 148, 112, 156, 84, 6, 112, 62, 142, 30, 226, 24, 19, 239, 233, 184, 76, 24, 234, 158, 194, 76, 147, 231, 99, 32, 14, 225, 190, 88, 140, 177, 181, 10, 203, 253, 222, 4, 240, 79, 190, 119, 176, 196, 98, 30, 73, 46, 208 }, new byte[] { 238, 9, 148, 94, 17, 247, 95, 110, 156, 188, 236, 203, 174, 168, 197, 165, 235, 252, 156, 198, 169, 73, 127, 20, 213, 100, 156, 64, 169, 159, 187, 81, 134, 51, 106, 113, 227, 108, 173, 102, 127, 233, 51, 139, 227, 45, 113, 116, 141, 203, 201, 211, 55, 246, 140, 160, 132, 186, 2, 90, 219, 155, 132, 254, 140, 4, 243, 245, 209, 244, 221, 239, 111, 102, 4, 12, 75, 16, 228, 37, 106, 113, 143, 50, 241, 107, 53, 179, 101, 128, 202, 143, 140, 28, 14, 37, 102, 19, 68, 27, 68, 65, 165, 243, 55, 57, 131, 233, 32, 119, 67, 185, 8, 176, 71, 129, 89, 206, 191, 177, 52, 93, 139, 5, 207, 158, 238, 32 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8514), true, false, "Anuraj", new byte[] { 94, 41, 119, 167, 129, 27, 222, 69, 218, 199, 50, 178, 182, 176, 25, 16, 225, 196, 47, 165, 37, 115, 118, 88, 133, 181, 49, 35, 103, 110, 218, 149, 10, 55, 104, 142, 230, 144, 178, 85, 155, 3, 225, 33, 84, 82, 167, 219, 106, 23, 144, 190, 94, 121, 49, 87, 72, 100, 104, 172, 93, 168, 203, 0 }, new byte[] { 175, 12, 246, 63, 97, 54, 194, 23, 227, 198, 30, 115, 5, 73, 250, 196, 92, 161, 3, 31, 90, 238, 177, 69, 230, 168, 178, 17, 63, 247, 238, 48, 124, 43, 182, 143, 1, 136, 61, 146, 119, 54, 11, 96, 182, 141, 10, 153, 50, 64, 218, 53, 171, 29, 22, 154, 84, 6, 112, 28, 147, 61, 188, 224, 200, 142, 202, 18, 164, 242, 7, 134, 133, 50, 244, 34, 98, 139, 140, 40, 31, 203, 218, 146, 213, 54, 204, 75, 32, 206, 133, 210, 172, 63, 137, 82, 47, 243, 44, 190, 231, 42, 64, 245, 129, 213, 232, 235, 170, 238, 250, 22, 121, 55, 175, 73, 226, 92, 29, 69, 74, 122, 135, 249, 0, 214, 205, 62 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 24, 12, 22, 1, 456, DateTimeKind.Local).AddTicks(8512), true, false, "Smart Bus", new byte[] { 90, 19, 99, 40, 23, 255, 135, 159, 14, 49, 206, 240, 2, 68, 137, 56, 178, 20, 88, 252, 82, 46, 237, 111, 122, 245, 60, 157, 165, 46, 89, 9, 159, 177, 116, 72, 161, 100, 12, 130, 194, 83, 212, 186, 201, 41, 63, 106, 57, 0, 79, 195, 119, 192, 18, 112, 245, 195, 221, 232, 149, 8, 16, 123 }, new byte[] { 123, 133, 78, 220, 192, 134, 0, 49, 9, 54, 176, 39, 26, 224, 165, 31, 201, 201, 61, 186, 174, 217, 243, 8, 98, 223, 31, 216, 14, 241, 135, 167, 26, 43, 39, 41, 237, 222, 219, 164, 166, 224, 126, 172, 177, 217, 65, 103, 173, 230, 248, 150, 202, 151, 1, 227, 5, 203, 9, 111, 8, 227, 139, 221, 227, 104, 93, 230, 176, 86, 116, 215, 72, 34, 244, 174, 152, 173, 120, 42, 216, 6, 224, 251, 121, 212, 35, 91, 186, 146, 120, 112, 251, 0, 56, 240, 121, 19, 112, 229, 202, 44, 31, 162, 242, 36, 244, 220, 66, 164, 52, 48, 220, 25, 106, 99, 124, 64, 111, 70, 7, 183, 200, 145, 32, 149, 14, 221 }, "TransportOperator" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Contact", "DOB", "Email", "Gender", "IsDeleted", "IsHandicapped", "Name" },
                values: new object[] { 1, "+911234567890", new DateOnly(2002, 7, 11), "anuraj@gmail.com", "Male", false, false, "Anuraj" });

            migrationBuilder.InsertData(
                table: "TransportOperators",
                columns: new[] { "Id", "Contact", "Email", "IsDeleted", "Name" },
                values: new object[] { 1, "+919876543210", "smartbus@gmail.com", false, "SmartBus" });

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "Id", "BusNo", "BusType", "OperatorId", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "TN01AB1234", "AC Sleeper", 1, 40 },
                    { 2, "TN01AB1235", "non-AC Seater", 1, 40 }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "ArrivalTime", "BusId", "BusRouteId", "DepartureTime", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 10, 20, 30, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 700m },
                    { 2, new DateTime(2025, 2, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 350m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TripId",
                table: "Bookings",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_OperatorId",
                table: "Buses",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPassengers_BookingId",
                table: "TicketPassengers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportOperators_Email",
                table: "TransportOperators",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusId",
                table: "Trips",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusRouteId",
                table: "Trips",
                column: "BusRouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TicketPassengers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "BusRoutes");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropTable(
                name: "TransportOperators");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
