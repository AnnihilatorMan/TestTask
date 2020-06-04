using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TestTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Data;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("exception")]
        public IActionResult Exception()
        {
            throw new NotImplementedException();
        }

        [HttpGet("logical_error")]
        public IActionResult LogicalError()
        {
            return BadRequest("Запрашиваемый пользователь не найден");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getinfo")]
        public IActionResult GetInfo()
        {
            var found = _context.Users.Find(uint.Parse(User.FindFirst("id").Value));
            return Ok(new { found.Name, found.BirthDate, found.Amount });
        }


        [HttpGet("auth")]
        public IActionResult Auth(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password" });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };
            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }

            return null;
        }
    }
}
