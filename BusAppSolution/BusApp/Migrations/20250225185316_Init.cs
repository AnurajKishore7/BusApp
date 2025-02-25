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
                    Distance = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    DepartureTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
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
                    JourneyDate = table.Column<DateTime>(type: "date", nullable: false),
                    TicketCount = table.Column<int>(type: "int", nullable: false),
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
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsHandicapped = table.Column<bool>(type: "bit", nullable: false)
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
                columns: new[] { "Id", "Destination", "Distance", "EstimatedDuration", "IsDeleted", "Source" },
                values: new object[,]
                {
                    { 1, "Kanyakumari", 750, "12:30", false, "Chennai" },
                    { 2, "Chennai", 750, "12:30", false, "Kanyakumari" },
                    { 3, "Bangalore", 350, "06:00", false, "Chennai" },
                    { 4, "Chennai", 350, "06:00", false, "Bangalore" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "CreatedAt", "IsApproved", "IsDeleted", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { "admin@gmail.com", new DateTime(2025, 2, 26, 0, 23, 16, 89, DateTimeKind.Local).AddTicks(1403), true, false, "Super Admin", new byte[] { 53, 28, 189, 98, 175, 199, 195, 17, 242, 159, 180, 240, 49, 186, 255, 180, 243, 248, 223, 149, 155, 187, 55, 228, 43, 29, 19, 41, 93, 223, 179, 93, 22, 118, 212, 68, 41, 118, 104, 110, 49, 61, 219, 80, 24, 119, 91, 29, 22, 82, 86, 122, 31, 79, 87, 5, 155, 85, 67, 233, 198, 103, 166, 80 }, new byte[] { 142, 23, 189, 162, 4, 90, 242, 120, 121, 252, 39, 232, 255, 149, 208, 75, 220, 206, 223, 65, 19, 110, 188, 187, 119, 68, 38, 64, 42, 250, 229, 82, 81, 230, 168, 50, 58, 152, 215, 219, 212, 139, 182, 68, 195, 7, 251, 75, 144, 192, 75, 12, 151, 23, 10, 51, 10, 165, 18, 241, 225, 124, 253, 205, 15, 91, 234, 253, 248, 127, 124, 91, 45, 203, 103, 253, 22, 129, 252, 128, 220, 2, 230, 66, 182, 51, 88, 140, 138, 166, 148, 186, 207, 13, 187, 123, 213, 192, 226, 74, 171, 128, 124, 134, 10, 112, 112, 49, 182, 52, 142, 40, 164, 200, 160, 101, 76, 109, 43, 18, 78, 74, 212, 84, 143, 162, 55, 84 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 26, 0, 23, 16, 89, DateTimeKind.Local).AddTicks(1408), true, false, "Anuraj", new byte[] { 71, 120, 43, 237, 155, 255, 4, 249, 167, 143, 131, 215, 176, 191, 163, 223, 255, 48, 1, 48, 119, 76, 13, 193, 129, 223, 214, 27, 229, 210, 19, 86, 194, 219, 219, 81, 128, 195, 57, 175, 203, 231, 138, 192, 255, 202, 77, 116, 204, 87, 238, 189, 186, 215, 238, 125, 20, 255, 189, 144, 58, 53, 69, 118 }, new byte[] { 202, 146, 83, 106, 205, 231, 108, 248, 75, 16, 24, 0, 100, 249, 127, 43, 142, 101, 210, 193, 248, 212, 198, 141, 16, 166, 103, 124, 193, 38, 206, 107, 46, 70, 240, 7, 208, 85, 227, 239, 86, 129, 176, 227, 198, 7, 203, 14, 249, 57, 179, 229, 83, 147, 216, 76, 179, 97, 105, 107, 15, 152, 30, 237, 43, 157, 217, 225, 141, 172, 197, 231, 176, 136, 247, 124, 215, 254, 249, 53, 202, 236, 116, 147, 145, 64, 0, 145, 147, 46, 70, 103, 59, 53, 245, 23, 129, 10, 171, 49, 58, 63, 31, 108, 64, 226, 76, 152, 85, 232, 185, 80, 255, 172, 168, 11, 15, 178, 71, 131, 69, 211, 105, 101, 126, 116, 91, 147 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 26, 0, 23, 16, 89, DateTimeKind.Local).AddTicks(1406), true, false, "Smart Bus", new byte[] { 199, 18, 156, 241, 79, 19, 135, 94, 171, 148, 22, 40, 41, 233, 16, 222, 143, 12, 203, 169, 215, 4, 46, 235, 253, 236, 8, 190, 198, 137, 126, 229, 197, 189, 128, 137, 173, 147, 150, 169, 111, 226, 48, 135, 223, 190, 181, 213, 150, 17, 108, 8, 76, 144, 91, 239, 128, 33, 226, 195, 74, 108, 157, 0 }, new byte[] { 206, 131, 39, 42, 27, 121, 88, 18, 216, 131, 68, 189, 233, 118, 38, 6, 100, 93, 43, 112, 159, 61, 25, 92, 100, 29, 180, 227, 228, 195, 115, 124, 65, 13, 45, 32, 137, 128, 216, 60, 126, 16, 161, 128, 201, 24, 200, 139, 49, 4, 33, 59, 96, 206, 214, 96, 141, 148, 73, 130, 92, 46, 182, 216, 218, 249, 225, 243, 112, 140, 54, 228, 94, 169, 98, 76, 11, 150, 121, 30, 251, 176, 152, 168, 197, 151, 240, 212, 64, 174, 35, 116, 51, 115, 212, 107, 68, 72, 154, 191, 168, 13, 162, 189, 241, 119, 115, 48, 67, 179, 94, 180, 232, 221, 90, 210, 150, 227, 180, 145, 219, 174, 121, 61, 114, 180, 241, 190 }, "TransportOperator" }
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
                columns: new[] { "Id", "BusNo", "BusType", "IsDeleted", "OperatorId", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "TN01AB1234", "AC Sleeper", false, 1, 40 },
                    { 2, "TN01AB1235", "non-AC Seater", false, 1, 40 }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "ArrivalTime", "BusId", "BusRouteId", "DepartureTime", "IsDeleted", "Price" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 20, 30, 0, 0), 1, 1, new TimeSpan(0, 8, 0, 0, 0), false, 700m },
                    { 2, new TimeSpan(0, 14, 0, 0, 0), 2, 3, new TimeSpan(0, 8, 0, 0, 0), false, 350m }
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
