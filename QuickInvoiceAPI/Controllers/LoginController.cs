using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickInvoiceAPI.Models;
using Microsoft.AspNetCore.Cors;
using QuickInvoiceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Cryptography;
using System.IO;

namespace QuickInvoiceAPI.Controllers
{
    [EnableCors("RulesCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly QuickInvoiceBdContext _bdContext;

        public LoginController(IConfiguration configuration, QuickInvoiceBdContext context)
        {
            _configuration = configuration;
            _bdContext = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            UserDTO? user = AuthenticateUser(login);

            if (user != null)
            {
                string tokenString = GenerateJSONWebToken();
                return Ok(tokenString);
            }

            return Unauthorized();
        }

        private UserDTO? AuthenticateUser(LoginDTO login)
        {
            UserDTO? user = _bdContext.Users
                .Where(user =>
                    user.UserName == login.UserName &&
                    user.Password == EncryptPassword(login.Password))
                .Select(user => new UserDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                }).FirstOrDefault();

            return user;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.GetValue<string>("Jwt:Issuer"),
                _configuration.GetValue<string>("Jwt:Issuer"),
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
