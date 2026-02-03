using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.Helpers;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce_ASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthManager authManager;
        private readonly JwtHelper jwtHelper;
        private readonly PasswordHasher passwordHasher;
        public AuthController(AuthManager authManager, JwtHelper jwtHelper, PasswordHasher passwordHasher)
        {
            this.authManager = authManager;

            this.jwtHelper = jwtHelper;

            this.passwordHasher = passwordHasher;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]registerDto userdto)
        {
            var passwordHash = passwordHasher.Hash(userdto.Password);

            authManager.RegisterDto(userdto, passwordHash);
            return Created("", "User registered successfully");
        }
       
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] loginDto userdto)
        {
           
            
            var token = jwtHelper.Authenticate(userdto);

            if (token==null)return Unauthorized("Invalid email or password");
            
            return Ok( new { token });

        }
        
        [Authorize]
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> forgetPasswored([FromBody] loginDto userdto,string newPassword)
        {
             authManager.GetUserByEmail(userdto.email,newPassword);
            return Ok("Password reset link sent to your email");
        }
        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] loginDto userdto , string newPassword)
        {
            var token = jwtHelper.Authenticate(userdto);

            if (token == null) return Unauthorized("Invalid email or password");
            
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentException("New password cannot be empty");
            newPassword = newPassword.Trim();

            authManager.ResetPassword(userdto.email,newPassword);
            return Ok("Password reset successfully");
        }
       
    }
}
