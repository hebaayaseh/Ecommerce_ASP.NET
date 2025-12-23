using Ecommerce_ASP.NET.DTOs.User;
using Ecommerce_ASP.NET.Helpers;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Models;
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
        public async Task<IActionResult> Loging([FromBody] loginDto userdto)
        {
           
            
            var token = jwtHelper.Authenticate(userdto);

            if (token==null)return Unauthorized("Invalid email or password");
            
            return Ok( new { token });

        }
    }
}
