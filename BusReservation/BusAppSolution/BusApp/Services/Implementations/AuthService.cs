using BusApp.DTOs;
using BusApp.Models;
using BusApp.Repositories.Interfaces;
using BusApp.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BusApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> RegisterClient(ClientRegisterDto request)
        {
            if (await _authRepository.UserExists(request.Email))
                throw new Exception("User with this email already exists.");

            // Hash password
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Client",
                IsApproved = true,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            var client = new Client
            {
                Name = user.Name,
                Email = user.Email,
                DOB = request.DOB,
                Gender = request.Gender,
                Contact = request.Contact,
                IsHandicapped = request.IsHandicapped,
                IsDeleted = false // Default value
            };


            var createdUser = await _authRepository.RegisterClient(user, client);

            // Generate JWT Token
            string token = GenerateJwtToken(createdUser);

            return new LoginResponseDto
            {
                Name = createdUser.Name,
                Email = createdUser.Email,
                Role = createdUser.Role,
                Message = "Welcome to Bus Ticket Booking App",
                Token = token
            };
        }

        public async Task<string> RegisterTransportOperator(TransportRegisterDto request)
        {
            if (await _authRepository.UserExists(request.Email))
                throw new Exception("User with this email already exists.");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "TransportOperator",
                IsApproved = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _authRepository.RegisterTransportOperator(user);
            return "Transport Operator registered successfully. Waiting for admin approval.";
        }

        // Login user and generate JWT token
        public async Task<LoginResponseDto> Login(LoginDto request)
        {
            var user = await _authRepository.GetUserByEmail(request.Email);
            if (user == null || user.IsDeleted)
                throw new Exception("Invalid email or password.");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Invalid email or password.");

            string message;
            string token = null;
            if (!user.IsApproved && user.Role == "TransportOperator")
            {
                message = "Your account is awaiting admin approval.";
            }
            else if (!user.IsApproved)
            {
                throw new Exception("User is not approved by admin.");
            }
            else
            {
                token = GenerateJwtToken(user);
                message = user.Role switch
                {
                    "Admin" => "Welcome to Admin Dashboard",
                    "Client" => "Welcome to Bus Ticket Booking App",
                    "TransportOperator" => "Welcome to Transport Operator Dashboard",
                    _ => "Unauthorized access"
                };
            }

            return new LoginResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Message = message,
                Token = token,
            };
        }

        public async Task<List<PendingOperatorDto>> GetPendingOperators()
        {
            var users = await _authRepository.GetUsersByRoleAndApproval("TransportOperator", false);
            return users.Select(u => new PendingOperatorDto
            {
                Name = u.Name,
                Email = u.Email
            }).ToList();
        }

        public async Task<bool> ApproveTransportOperator(string name)
        {
            bool result = await _authRepository.ApproveTransportOperator(name);
            if (!result)
                throw new Exception("Approval failed. Either user does not exist or is already approved.");

            return result;
        }

        // Helper: Create password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Helper: Verify password hash
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        // Helper: Generate JWT token
        private string GenerateJwtToken(User user)
        {
            var secretKey = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("JWT secret key is missing in configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.Email), // Email as primary key
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role)
};

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
