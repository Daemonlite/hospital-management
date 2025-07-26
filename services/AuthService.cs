using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Azure.Core;
using Health.Data;
using Health.Entities;
using Health.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Health.services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http.HttpResults;


namespace Health.services
{
    public class AuthService(AppDbContext context, IConfiguration configuration, RedisCacheService _cache, EmailService mail) : IAuthService
    {
        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            return await context.Users
                .Include(u => u.Department)
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    CreatedAt = u.CreatedAt,
                    Department = u.Department != null ? new DepartmentDto
                    {
                        Id = u.Department.Id,
                        Name = u.Department.Name
                    } : null

                })
                .ToListAsync();
        }

        public async Task<UserListDto?> GetUserById(Guid id)
        {
            var user = await context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                return null;
            }
            return new UserListDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                Department = user.Department != null ? new DepartmentDto
                {
                    Id = user.Department.Id,
                    Name = user.Department.Name
                } : null

            };
        }

        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            TokenResponseDto response = await CreateTokenResponse(user);

            return response;
        }

        public async Task<UserListDto?> RegisterAsync(UserCreateDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return null;
            }

            if (await context.Departments.AnyAsync(d => d.Id != request.DepartmentId))
            {
                throw new ArgumentException("Department does not exist");
            }

            // Only verify department if DepartmentId is provided
            if (request.DepartmentId.HasValue &&
                !await context.Departments.AnyAsync(d => d.Id == request.DepartmentId))
            {
                throw new ArgumentException("Department does not exist");
            }


            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = new PasswordHasher<User>().HashPassword(new User(), request.Password),
                Role = Enum.Parse<Role>(request.Role),
                DepartmentId = request.DepartmentId.HasValue ? request.DepartmentId.Value : null
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return await GetUserById(user.Id);
        }


        private string CreateToken(User user)
        {
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.FullName),
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, user.Role.ToString())
                };

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!
                    ));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: configuration.GetValue<string>("AppSettings:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: creds
                );
                return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            }
            return string.Empty;
        }


        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await context.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;


        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        public async Task<User?> UpdateUserAsync(Guid id, UserUpdateDto request)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                user.FullName = request.FullName;
                user.Role = request.Role;
                user.Email = request.Email;
                await context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public async Task<User?> DeleteUserAsync(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        private static string GenerateOtp()
        {
            var randomNumber = new byte[6];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool?> SendOtpAsync(SendOtpDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                var otp = GenerateOtp();
                await _cache.CacheSetAsync(request.Email, otp);
                await mail.SendEmailAsync(request.Email, "OTP", $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ color: #2c3e50; border-bottom: 1px solid #eee; padding-bottom: 10px; }}
                            .otp-container {{ background: #f9f9f9; padding: 20px; text-align: center; margin: 20px 0; border-radius: 5px; }}
                            .otp-code {{ font-size: 24px; font-weight: bold; letter-spacing: 3px; color: #2c3e50; }}
                            .footer {{ margin-top: 20px; font-size: 12px; color: #7f8c8d; text-align: center; }}
                            .warning {{ color: #e74c3c; font-weight: bold; }}
                        </style>
                    </head>
                    <body>
                        <div class='header'>
                            <h2>One-Time Password (OTP)</h2>
                        </div>
                        
                        <p>Hello {user.FullName},</p>
                        
                        <p>Your one-time password for verification is:</p>
                        
                        <div class='otp-container'>
                            <div class='otp-code'>{otp}</div>
                        </div>
                        
                        <p class='warning'>This OTP is valid for 5 minutes. Please do not share this code with anyone.</p>
                        
                        <p>If you didn't request this OTP, please ignore this email or contact support.</p>
                        
                        <div class='footer'>
                            <p>© {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
                        </div>
                    </body>
                    </html>");
                return true;

            }
            return false;
        }


        public async Task<User?> ResetPasswordAsync(ResetPasswordDto request)
        {

            if (request.Password != request.ConfirmPassword)
            {
                return null;
            }
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user != null)
            {
                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
                await context.SaveChangesAsync();
                return user;
            }

            return null;
        }


    }
}
