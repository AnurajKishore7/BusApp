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
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
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
                    { "admin@gmail.com", new DateTime(2025, 2, 23, 23, 8, 22, 848, DateTimeKind.Local).AddTicks(2109), true, false, "Super Admin", new byte[] { 118, 51, 142, 51, 49, 71, 235, 198, 203, 201, 198, 127, 210, 142, 48, 217, 69, 97, 144, 239, 220, 48, 124, 211, 72, 99, 115, 225, 62, 218, 71, 26, 138, 120, 122, 219, 68, 196, 199, 122, 141, 16, 163, 45, 129, 4, 243, 146, 244, 15, 49, 89, 29, 46, 38, 22, 30, 183, 23, 188, 151, 199, 161, 69 }, new byte[] { 83, 179, 220, 219, 22, 220, 96, 73, 200, 175, 96, 35, 138, 120, 237, 200, 114, 58, 119, 143, 36, 226, 44, 8, 49, 18, 139, 79, 165, 162, 33, 161, 136, 24, 233, 160, 254, 94, 161, 94, 163, 19, 89, 90, 65, 37, 35, 90, 71, 100, 208, 58, 59, 3, 176, 230, 29, 10, 147, 246, 128, 57, 228, 84, 177, 10, 20, 115, 166, 77, 53, 156, 221, 245, 221, 68, 239, 180, 215, 186, 43, 163, 246, 111, 183, 93, 177, 31, 210, 177, 114, 26, 132, 177, 92, 155, 55, 222, 10, 19, 189, 36, 181, 9, 181, 160, 166, 60, 131, 28, 134, 35, 185, 93, 194, 115, 254, 178, 77, 49, 241, 31, 202, 36, 241, 172, 205, 175 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 23, 23, 8, 22, 848, DateTimeKind.Local).AddTicks(2114), true, false, "Anuraj", new byte[] { 65, 127, 237, 234, 97, 122, 40, 53, 91, 244, 15, 56, 141, 8, 112, 207, 57, 133, 90, 77, 183, 201, 212, 205, 107, 105, 234, 219, 233, 78, 4, 136, 37, 101, 180, 125, 168, 106, 180, 79, 176, 205, 14, 116, 134, 177, 98, 173, 36, 19, 180, 181, 117, 103, 45, 158, 105, 121, 208, 121, 87, 144, 144, 208 }, new byte[] { 150, 114, 55, 198, 109, 92, 214, 182, 37, 234, 173, 100, 140, 116, 220, 159, 245, 24, 142, 86, 55, 62, 244, 158, 231, 8, 84, 110, 192, 110, 65, 35, 38, 231, 78, 198, 52, 17, 126, 149, 86, 225, 88, 47, 56, 116, 120, 216, 240, 64, 65, 66, 204, 145, 43, 108, 168, 245, 249, 184, 228, 211, 161, 87, 98, 220, 40, 2, 136, 132, 114, 243, 104, 176, 133, 48, 196, 60, 172, 45, 57, 28, 71, 199, 29, 81, 12, 132, 119, 149, 18, 98, 201, 125, 6, 39, 219, 167, 210, 139, 187, 69, 16, 220, 222, 56, 8, 193, 182, 252, 212, 208, 97, 201, 103, 166, 143, 234, 175, 176, 118, 126, 129, 183, 193, 47, 6, 104 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 23, 23, 8, 22, 848, DateTimeKind.Local).AddTicks(2112), true, false, "Smart Bus", new byte[] { 71, 219, 53, 2, 182, 22, 233, 210, 205, 62, 95, 220, 194, 4, 236, 36, 138, 100, 161, 59, 179, 252, 84, 147, 56, 199, 216, 99, 12, 209, 156, 10, 111, 224, 234, 132, 181, 63, 205, 229, 156, 121, 255, 154, 62, 92, 59, 190, 170, 76, 17, 192, 115, 26, 110, 78, 213, 242, 131, 220, 203, 106, 168, 60 }, new byte[] { 213, 112, 125, 210, 238, 251, 65, 88, 193, 228, 247, 80, 168, 130, 219, 181, 173, 108, 68, 178, 242, 114, 166, 209, 217, 174, 229, 96, 56, 31, 27, 112, 34, 251, 214, 239, 208, 240, 251, 202, 12, 254, 119, 16, 178, 231, 50, 28, 110, 167, 43, 223, 68, 90, 18, 24, 22, 244, 135, 113, 179, 70, 178, 121, 172, 206, 63, 161, 194, 131, 192, 197, 120, 123, 216, 184, 238, 242, 214, 153, 255, 87, 71, 42, 69, 23, 224, 17, 212, 117, 202, 43, 181, 216, 255, 221, 193, 39, 33, 249, 25, 248, 82, 175, 103, 158, 17, 107, 143, 203, 218, 111, 215, 200, 20, 89, 202, 251, 29, 99, 171, 71, 29, 31, 124, 218, 176, 234 }, "TransportOperator" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Contact", "DOB", "Email", "Gender", "IsDeleted", "IsDisabled", "Name" },
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
