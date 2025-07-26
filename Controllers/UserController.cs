using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Health.Entities;
using Health.Models;
using Health.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Health.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authservice) : ControllerBase
    {


        [HttpGet("{id}")]
        public async Task<ActionResult<UserListDto>> GetUser(Guid id)
        {
            var user = await authservice.GetUserById(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserListDto>>> GetAllUsersAsync()
        {
            return Ok(await authservice.GetAllUsersAsync());
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserListDto>> Register(UserCreateDto request)
        {

            if (!Enum.TryParse(request.Role, ignoreCase: true, out Role role))
            {
                return BadRequest("Invalid role passed");
            }
            var user = await authservice.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("User already exists");
            }
            else
            {
                return Ok(user);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto request)
        {
            var result = await authservice.LoginAsync(request);

            if(result == null)
            {
                return BadRequest("Invalid credentials");
            }
            else
            {
                return Ok(result);
            }

        }

        [HttpPost("update-user/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> UpdateUser(Guid id, UserUpdateDto request)
        {
           
            var user = await authservice.UpdateUserAsync(id,request);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                return Ok(user);
            }

            
        }

        [HttpPost("delete-user/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var user = await authservice.DeleteUserAsync(id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost("send-otp")]
        public async Task<ActionResult<bool>> SendOtp(SendOtpDto email)
        {
            var otp = await authservice.SendOtpAsync(email);
            if (otp == false)
            {
                return BadRequest("User not found");
            }
            else
            {
                return Ok("Otp sent successfully");
            }
           
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<User>> ResetPassword(ResetPasswordDto request)
        {
            var user = await authservice.ResetPasswordAsync(request);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            else
            {
                return Ok(user);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdmindOnlyEndpoint()
        {
            return Ok("You are an authenticated admin !");
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authservice.RefreshTokensAsync(request);
            if(result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid RefreshToken");
            }
            return Ok(result);
        }
    }
}
