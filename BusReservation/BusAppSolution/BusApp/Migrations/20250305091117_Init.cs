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
                    EstimatedDuration = table.Column<TimeSpan>(type: "time", nullable: false),
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
                name: "BusSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusSeats_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
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
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BookedOn = table.Column<DateTime>(type: "datetime", nullable: false)
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
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsHandicapped = table.Column<bool>(type: "bit", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
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
                table: "Users",
                columns: new[] { "Email", "CreatedAt", "IsApproved", "IsDeleted", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { "admin@gmail.com", new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3084), true, false, "Super Admin", new byte[] { 116, 88, 87, 25, 97, 134, 104, 24, 17, 3, 51, 58, 104, 67, 124, 92, 240, 128, 81, 64, 69, 159, 129, 206, 224, 144, 52, 45, 186, 35, 142, 18, 171, 73, 169, 70, 89, 50, 32, 117, 96, 182, 178, 250, 129, 69, 68, 81, 38, 129, 127, 48, 27, 76, 209, 209, 108, 31, 194, 158, 100, 17, 37, 42 }, new byte[] { 221, 8, 42, 12, 46, 239, 136, 251, 113, 220, 75, 227, 81, 108, 119, 93, 28, 162, 226, 118, 75, 112, 21, 132, 55, 41, 188, 126, 160, 61, 216, 182, 195, 145, 254, 84, 10, 130, 70, 189, 108, 167, 96, 181, 240, 88, 94, 222, 43, 229, 19, 84, 169, 162, 147, 39, 135, 99, 29, 164, 40, 94, 165, 154, 175, 227, 123, 95, 255, 200, 135, 39, 149, 139, 217, 16, 188, 221, 192, 216, 82, 148, 29, 218, 140, 32, 234, 205, 73, 136, 201, 214, 230, 241, 77, 42, 252, 18, 41, 186, 32, 40, 212, 254, 198, 208, 18, 183, 148, 137, 117, 9, 235, 26, 43, 7, 119, 127, 42, 2, 130, 127, 40, 210, 222, 233, 239, 206 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3087), true, false, "Anuraj", new byte[] { 43, 62, 56, 214, 243, 254, 242, 231, 84, 35, 200, 31, 199, 45, 149, 202, 128, 188, 28, 214, 7, 249, 117, 66, 165, 133, 135, 252, 89, 87, 200, 162, 206, 172, 22, 128, 51, 20, 224, 172, 13, 98, 3, 168, 244, 95, 135, 234, 84, 170, 141, 7, 149, 237, 183, 106, 71, 120, 105, 45, 222, 191, 103, 39 }, new byte[] { 15, 94, 156, 47, 13, 224, 22, 113, 38, 44, 57, 114, 23, 57, 194, 184, 120, 56, 174, 193, 103, 35, 51, 140, 29, 83, 90, 171, 2, 46, 128, 192, 75, 32, 225, 87, 155, 74, 218, 211, 97, 136, 206, 113, 61, 152, 186, 245, 171, 199, 103, 249, 207, 160, 141, 145, 214, 207, 211, 64, 29, 122, 206, 32, 80, 96, 105, 168, 89, 139, 175, 91, 33, 214, 25, 239, 47, 207, 155, 198, 1, 174, 40, 48, 209, 183, 181, 230, 144, 123, 210, 160, 40, 42, 158, 168, 55, 176, 248, 236, 160, 16, 210, 56, 252, 95, 128, 21, 90, 150, 152, 15, 209, 59, 115, 233, 190, 241, 252, 9, 217, 181, 121, 41, 63, 223, 183, 150 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 3, 5, 14, 41, 16, 669, DateTimeKind.Local).AddTicks(3089), true, false, "SmartBus", new byte[] { 176, 74, 239, 75, 170, 167, 115, 2, 186, 234, 134, 116, 165, 244, 249, 40, 46, 160, 104, 49, 25, 82, 80, 215, 80, 230, 127, 21, 250, 5, 202, 193, 57, 122, 147, 55, 123, 196, 58, 75, 248, 16, 102, 99, 5, 21, 245, 30, 63, 193, 3, 169, 118, 33, 128, 92, 149, 231, 79, 225, 201, 2, 99, 164 }, new byte[] { 220, 169, 31, 19, 174, 68, 113, 76, 135, 243, 100, 247, 88, 26, 154, 14, 157, 80, 102, 94, 254, 165, 145, 58, 18, 39, 26, 154, 169, 205, 93, 144, 158, 66, 76, 185, 155, 48, 177, 187, 237, 140, 0, 161, 173, 111, 180, 239, 107, 152, 154, 80, 139, 110, 129, 129, 35, 234, 175, 63, 73, 51, 137, 13, 183, 50, 161, 126, 126, 106, 176, 122, 47, 142, 113, 174, 18, 245, 80, 69, 176, 153, 159, 114, 31, 147, 119, 239, 164, 67, 41, 97, 208, 130, 240, 39, 154, 76, 148, 67, 241, 110, 207, 2, 105, 69, 135, 78, 86, 179, 49, 71, 232, 15, 229, 54, 176, 79, 209, 194, 184, 11, 196, 228, 237, 142, 119, 76 }, "TransportOperator" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Contact", "DOB", "Email", "Gender", "IsDeleted", "IsHandicapped", "Name" },
                values: new object[] { 1, "+911234567890", new DateOnly(2002, 7, 11), "anuraj@gmail.com", "Male", false, false, "Anuraj" });

            migrationBuilder.InsertData(
                table: "TransportOperators",
                columns: new[] { "Id", "Contact", "Email", "IsDeleted", "Name" },
                values: new object[] { 1, "+919876543210", "smartbus@gmail.com", false, "SmartBus" });

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
                name: "IX_BusSeats_BusId_SeatNumber",
                table: "BusSeats",
                columns: new[] { "BusId", "SeatNumber" },
                unique: true);

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
                name: "BusSeats");

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
