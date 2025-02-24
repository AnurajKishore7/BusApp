﻿// <auto-generated />
using System;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250225050010_IsDeleted")]
    partial class IsDeleted
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BusApp.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("TripId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("BusApp.Models.Bus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BusNo")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("BusType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("OperatorId")
                        .HasColumnType("int");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OperatorId");

                    b.ToTable("Buses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BusNo = "TN01AB1234",
                            BusType = "AC Sleeper",
                            IsDeleted = false,
                            OperatorId = 1,
                            TotalSeats = 40
                        },
                        new
                        {
                            Id = 2,
                            BusNo = "TN01AB1235",
                            BusType = "non-AC Seater",
                            IsDeleted = false,
                            OperatorId = 1,
                            TotalSeats = 40
                        });
                });

            modelBuilder.Entity("BusApp.Models.BusRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<string>("EstimatedDuration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("BusRoutes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Destination = "Kanyakumari",
                            Distance = 750,
                            EstimatedDuration = "12:30",
                            IsDeleted = false,
                            Source = "Chennai"
                        },
                        new
                        {
                            Id = 2,
                            Destination = "Chennai",
                            Distance = 750,
                            EstimatedDuration = "12:30",
                            IsDeleted = false,
                            Source = "Kanyakumari"
                        },
                        new
                        {
                            Id = 3,
                            Destination = "Bangalore",
                            Distance = 350,
                            EstimatedDuration = "06:00",
                            IsDeleted = false,
                            Source = "Chennai"
                        },
                        new
                        {
                            Id = 4,
                            Destination = "Chennai",
                            Distance = 350,
                            EstimatedDuration = "06:00",
                            IsDeleted = false,
                            Source = "Bangalore"
                        });
                });

            modelBuilder.Entity("BusApp.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("DOB")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsHandicapped")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Contact = "+911234567890",
                            DOB = new DateOnly(2002, 7, 11),
                            Email = "anuraj@gmail.com",
                            Gender = "Male",
                            IsDeleted = false,
                            IsHandicapped = false,
                            Name = "Anuraj"
                        });
                });

            modelBuilder.Entity("BusApp.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentMadeAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("BusApp.Models.TicketPassenger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SeatNo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("TicketPassengers");
                });

            modelBuilder.Entity("BusApp.Models.TransportOperator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("TransportOperators");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Contact = "+919876543210",
                            Email = "smartbus@gmail.com",
                            IsDeleted = false,
                            Name = "SmartBus"
                        });
                });

            modelBuilder.Entity("BusApp.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<int>("BusRouteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("BusId");

                    b.HasIndex("BusRouteId");

                    b.ToTable("Trips");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArrivalTime = new DateTime(2025, 2, 10, 20, 30, 0, 0, DateTimeKind.Unspecified),
                            BusId = 1,
                            BusRouteId = 1,
                            DepartureTime = new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            Price = 700m
                        },
                        new
                        {
                            Id = 2,
                            ArrivalTime = new DateTime(2025, 2, 10, 14, 0, 0, 0, DateTimeKind.Unspecified),
                            BusId = 2,
                            BusRouteId = 3,
                            DepartureTime = new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false,
                            Price = 350m
                        });
                });

            modelBuilder.Entity("BusApp.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Email = "admin@gmail.com",
                            CreatedAt = new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2326),
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Super Admin",
                            PasswordHash = new byte[] { 17, 132, 67, 69, 109, 37, 147, 94, 50, 173, 115, 251, 106, 247, 23, 156, 68, 229, 95, 79, 211, 200, 42, 74, 235, 1, 108, 157, 42, 115, 12, 81, 4, 76, 144, 53, 54, 38, 223, 5, 53, 208, 226, 213, 122, 242, 117, 90, 114, 14, 144, 188, 117, 33, 42, 232, 78, 181, 26, 18, 90, 28, 54, 64 },
                            PasswordSalt = new byte[] { 254, 164, 51, 53, 22, 253, 88, 249, 254, 40, 96, 31, 241, 34, 188, 193, 127, 60, 92, 221, 25, 41, 217, 73, 116, 68, 134, 203, 17, 42, 61, 103, 23, 250, 214, 153, 195, 49, 72, 185, 253, 28, 54, 28, 14, 169, 37, 166, 57, 134, 66, 249, 154, 223, 249, 108, 215, 190, 44, 84, 84, 84, 88, 70, 7, 215, 33, 57, 243, 247, 165, 123, 170, 95, 71, 88, 172, 184, 181, 43, 95, 55, 166, 144, 104, 59, 111, 164, 144, 13, 248, 182, 200, 56, 255, 10, 97, 178, 202, 246, 115, 33, 79, 56, 78, 242, 123, 163, 83, 200, 37, 197, 111, 91, 115, 88, 131, 17, 128, 183, 64, 104, 27, 146, 56, 9, 216, 5 },
                            Role = "Admin"
                        },
                        new
                        {
                            Email = "smartbus@gmail.com",
                            CreatedAt = new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2382),
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Smart Bus",
                            PasswordHash = new byte[] { 13, 100, 239, 85, 80, 178, 117, 166, 123, 26, 141, 13, 89, 211, 203, 15, 73, 80, 112, 96, 161, 128, 82, 144, 115, 127, 117, 165, 49, 80, 250, 140, 43, 89, 184, 108, 249, 174, 62, 126, 166, 116, 20, 229, 243, 247, 170, 107, 195, 183, 220, 255, 80, 31, 197, 86, 169, 116, 163, 96, 170, 208, 218, 74 },
                            PasswordSalt = new byte[] { 183, 179, 54, 138, 240, 3, 199, 220, 117, 247, 142, 4, 120, 185, 233, 30, 67, 94, 1, 215, 13, 143, 62, 198, 90, 62, 69, 163, 60, 13, 120, 121, 248, 134, 186, 148, 44, 2, 163, 203, 80, 54, 71, 20, 240, 142, 199, 98, 24, 134, 62, 101, 121, 153, 93, 65, 12, 200, 42, 131, 202, 116, 37, 36, 222, 165, 123, 61, 233, 28, 40, 221, 115, 148, 207, 240, 253, 16, 251, 122, 36, 68, 37, 230, 224, 110, 154, 47, 9, 132, 173, 118, 78, 180, 172, 131, 3, 35, 68, 92, 76, 238, 130, 8, 42, 66, 87, 30, 136, 136, 90, 132, 175, 113, 177, 77, 116, 58, 180, 22, 181, 90, 96, 98, 5, 172, 168, 16 },
                            Role = "TransportOperator"
                        },
                        new
                        {
                            Email = "anuraj@gmail.com",
                            CreatedAt = new DateTime(2025, 2, 25, 10, 30, 9, 573, DateTimeKind.Local).AddTicks(2385),
                            IsApproved = true,
                            IsDeleted = false,
                            Name = "Anuraj",
                            PasswordHash = new byte[] { 172, 236, 228, 8, 233, 111, 200, 164, 103, 177, 238, 0, 16, 16, 118, 208, 140, 99, 219, 41, 178, 214, 46, 187, 41, 27, 117, 76, 84, 148, 2, 15, 161, 206, 206, 221, 231, 61, 255, 74, 218, 237, 250, 126, 68, 62, 208, 192, 172, 181, 47, 229, 70, 115, 68, 61, 29, 144, 142, 182, 118, 66, 183, 188 },
                            PasswordSalt = new byte[] { 37, 55, 7, 242, 161, 185, 186, 194, 148, 160, 91, 166, 193, 184, 218, 89, 20, 162, 54, 60, 245, 251, 206, 91, 214, 9, 112, 177, 184, 32, 26, 119, 237, 177, 184, 34, 117, 100, 3, 21, 254, 90, 178, 210, 196, 35, 138, 174, 122, 61, 163, 87, 162, 67, 198, 240, 172, 52, 60, 216, 210, 251, 68, 111, 187, 32, 169, 75, 213, 80, 147, 247, 129, 15, 144, 49, 14, 152, 13, 43, 170, 21, 40, 216, 193, 132, 178, 156, 211, 137, 23, 243, 217, 208, 248, 128, 183, 29, 204, 119, 88, 169, 67, 245, 26, 242, 175, 65, 152, 195, 136, 211, 181, 212, 99, 223, 91, 211, 188, 55, 21, 93, 24, 80, 212, 220, 239, 238 },
                            Role = "Client"
                        });
                });

            modelBuilder.Entity("BusApp.Models.Booking", b =>
                {
                    b.HasOne("BusApp.Models.Client", "Client")
                        .WithMany("Bookings")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusApp.Models.Trip", "Trip")
                        .WithMany("Bookings")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("BusApp.Models.Bus", b =>
                {
                    b.HasOne("BusApp.Models.TransportOperator", "TransportOperator")
                        .WithMany("Buses")
                        .HasForeignKey("OperatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TransportOperator");
                });

            modelBuilder.Entity("BusApp.Models.Client", b =>
                {
                    b.HasOne("BusApp.Models.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("BusApp.Models.Client", "Email")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusApp.Models.Payment", b =>
                {
                    b.HasOne("BusApp.Models.Booking", "Booking")
                        .WithOne("Payment")
                        .HasForeignKey("BusApp.Models.Payment", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BusApp.Models.TicketPassenger", b =>
                {
                    b.HasOne("BusApp.Models.Booking", "Booking")
                        .WithMany("TicketPassengers")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("BusApp.Models.TransportOperator", b =>
                {
                    b.HasOne("BusApp.Models.User", "User")
                        .WithOne("TransportOperator")
                        .HasForeignKey("BusApp.Models.TransportOperator", "Email")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusApp.Models.Trip", b =>
                {
                    b.HasOne("BusApp.Models.Bus", "Bus")
                        .WithMany("Trips")
                        .HasForeignKey("BusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusApp.Models.BusRoute", "BusRoute")
                        .WithMany("Trips")
                        .HasForeignKey("BusRouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("BusRoute");
                });

            modelBuilder.Entity("BusApp.Models.Booking", b =>
                {
                    b.Navigation("Payment");

                    b.Navigation("TicketPassengers");
                });

            modelBuilder.Entity("BusApp.Models.Bus", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("BusApp.Models.BusRoute", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("BusApp.Models.Client", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BusApp.Models.TransportOperator", b =>
                {
                    b.Navigation("Buses");
                });

            modelBuilder.Entity("BusApp.Models.Trip", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("BusApp.Models.User", b =>
                {
                    b.Navigation("Client");

                    b.Navigation("TransportOperator");
                });
#pragma warning restore 612, 618
        }
    }
}
