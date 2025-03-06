using System.Security.Cryptography;
using System.Text;
using BusApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TransportOperator> TransportOperators { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusSeat> BusSeats { get; set; }
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TicketPassenger> TicketPassengers { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>().HasKey(u => u.Email);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("nvarchar(255)");
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("varbinary(max)");
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordSalt)
                .HasColumnType("varbinary(max)");

            // User - TransportOperator (One-to-One)
            modelBuilder.Entity<TransportOperator>()
                .Property(to => to.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TransportOperator>()
                .HasOne(to => to.User)
                .WithOne(u => u.TransportOperator)
                .HasForeignKey<TransportOperator>(to => to.Email)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Client (One-to-One)
            modelBuilder.Entity<Client>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.Email)
                .OnDelete(DeleteBehavior.NoAction);

            // TransportOperator - Buses (One-to-Many)
            modelBuilder.Entity<Bus>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.TransportOperator)
                .WithMany(to => to.Buses)
                .HasForeignKey(b => b.OperatorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bus - BusSeats (One-to-Many)
            modelBuilder.Entity<BusSeat>()
                .Property(bs => bs.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<BusSeat>()
                .HasOne(bs => bs.Bus)
                .WithMany() // No navigation property in Bus for BusSeats yet
                .HasForeignKey(bs => bs.BusId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BusSeat>()
                .HasIndex(bs => new { bs.BusId, bs.SeatNumber }) // Unique constraint
                .IsUnique();

            // BusRoute - Trips (One-to-Many)
            modelBuilder.Entity<BusRoute>()
                .Property(br => br.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<BusRoute>()
                .Property(br => br.EstimatedDuration)
                .HasColumnType("time"); // Updated to TimeSpan
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.BusRoute)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.BusRouteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bus - Trips (One-to-Many)
            modelBuilder.Entity<Trip>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client - Bookings (One-to-Many)
            modelBuilder.Entity<Booking>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip - Bookings (One-to-Many)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking - TicketPassengers (One-to-Many)
            modelBuilder.Entity<TicketPassenger>()
                .Property(tp => tp.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TicketPassenger>()
                .HasOne(tp => tp.Booking)
                .WithMany(b => b.TicketPassengers)
                .HasForeignKey(tp => tp.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking - Payment (One-to-One)
            modelBuilder.Entity<Payment>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data (Admin and Client only)
            using var hmacAdmin = new HMACSHA512();
            var adminHash = hmacAdmin.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));
            var adminSalt = hmacAdmin.Key;

            using var hmacClient = new HMACSHA512();
            var clientHash = hmacClient.ComputeHash(Encoding.UTF8.GetBytes("anuraj@123"));
            var clientSalt = hmacClient.Key;

            using var hmacOperator = new HMACSHA512();
            var operatorHash = hmacOperator.ComputeHash(Encoding.UTF8.GetBytes("operator@123"));
            var operatorSalt = hmacOperator.Key;

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "admin@gmail.com",
                    Name = "Super Admin",
                    PasswordHash = adminHash,
                    PasswordSalt = adminSalt,
                    Role = "Admin",
                    IsApproved = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Email = "anuraj@gmail.com",
                    Name = "Anuraj",
                    PasswordHash = clientHash,
                    PasswordSalt = clientSalt,
                    Role = "Client",
                    IsApproved = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Email = "smartbus@gmail.com",
                    Name = "SmartBus",
                    PasswordHash = operatorHash,
                    PasswordSalt = operatorSalt,
                    Role = "TransportOperator",
                    IsApproved = true,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false
                }

            );

            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Email = "anuraj@gmail.com",
                    Name = "Anuraj",
                    DOB = new DateOnly(2002, 07, 11),
                    Gender = "Male",
                    Contact = "+911234567890",
                    IsHandicapped = false
                }
            );

            modelBuilder.Entity<TransportOperator>().HasData(
                new TransportOperator
                {
                    Id = 1,
                    Email = "smartbus@gmail.com",
                    Name = "SmartBus",
                    Contact = "+919876543210",
                    IsDeleted = false
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
