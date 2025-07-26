using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Health.Entities;

namespace Health.Models
{
    
    public enum Role
    {
        Admin,          // System administrator
        Doctor,         // Medical doctor
        Nurse,         // Nursing staff
        Pharmacist,     // Pharmacy staff
        LabTechnician,  // Laboratory staff
        Receptionist,   // Front desk/reception
        // Add more roles as needed
    }

    public class UserCreateDto
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public required string Role { get; set; }

        public Guid? DepartmentId { get; set; }
    }

    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; } 
    }


    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

    }

    public class UserListDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DepartmentDto? Department { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class UserDto
    {

        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        
    }

   public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<UserDto> Users { get; set; } = [];
    }



    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class SendOtpDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
    }

    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public Guid UserId { get; set; }

        public required string RefreshToken { get; set; }
    }


}