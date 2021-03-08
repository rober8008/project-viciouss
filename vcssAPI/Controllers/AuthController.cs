using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using vcssAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace vcssAPI.Controllers
{
    [Produces("application/json")]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPost("register")]
        public JwtPacket Register([FromBody]mdlTenant user)
        {
            return CreateToken(user.Username);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]LoginData loginData)
        {
            if(loginData.Username == "aa")
            {
                return Ok(CreateToken(loginData.Username));
            }
            else
            {
                return NotFound("Invalid Username or Secretpass!");
            }
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            string id = HttpContext.User.Claims.First().Value;
            return Ok(id);
        }

        JwtPacket CreateToken(string username)
        {
            Claim[] claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, username) };
            SigningCredentials signingCredentials = new SigningCredentials(Startup.signingKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JwtPacket() { Token = encodedJwt, Email = username };
        }
    }

    public class JwtPacket
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }

    public class LoginData
    {
        public string Username { get; set; }
        public string Secretpass { get; set; }
    }
}