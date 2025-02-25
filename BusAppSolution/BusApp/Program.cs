using System.Text;
using BusApp.Models;
using BusApp.Repositories.Implementations;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Implementations;
using BusApp.Services.Interfaces;
using BusReservationApp.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BusApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var environment = builder.Environment;

            // Add services to the container
            #region NewtonsoftJson

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            #endregion

            // Add Swagger/OpenAPI
            #region Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            #endregion

            // Database Context Setup
            #region Database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'DefaultConnection' is missing in appsettings.json.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            #endregion

            // Register HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

            // Repository Layer Dependency Injection
            #region Repositories         
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IClientRepo, ClientRepo>();
            builder.Services.AddScoped<IOperatorRepo, OperatorRepo>();
            builder.Services.AddScoped<IBusesRepo,  BusesRepo>();
            builder.Services.AddScoped<IBusRoutesRepo, BusRoutesRepo>();
            #endregion

            // Service Layer Dependency Injection
            #region Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IOperatorService, OperatorService>();
            builder.Services.AddScoped<IBusesService, BusesService>();
            builder.Services.AddScoped<IBusRoutesService,  BusRoutesService>();
            #endregion

            // JWT Authentication & Authorization
            #region JWT Authentication & Authorization
            var jwtSecret = configuration["Jwt:Secret"];
            var key = Encoding.UTF8.GetBytes(jwtSecret);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        //ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = false,
                       // ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();
            #endregion

            var app = builder.Build();

            // Middleware 
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();

        }
    }
}
