using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ErestAPI.Repository;
using ErestAPI.Models;

namespace ErestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly AuthService _loginService;

        public AuthController(IConfiguration configuration, AuthService loginService)
        {
            _config = configuration;
            _loginService = loginService;
        }


        [HttpPost]
        public IActionResult Login([FromBody] UserModel login)
        {

            IActionResult result = Unauthorized();
            //var user = AuthenticateUser(login);
            var user = _loginService.SelectCurrentUser(login);
            if (user.Count != 0)
            {
                UserModel loginuser = new UserModel();
                user.ForEach(x => loginuser = x);
                var tokenStr = GenarateJsonWebToken(loginuser);
                result = Ok(new { token = tokenStr, user = loginuser });
            }
            return result;

        }

        private string GenarateJsonWebToken(UserModel userinfo)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var Credintional = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userinfo.EmaillAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userinfo.RoleName)
            };

            var token = new JwtSecurityToken(issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: Credintional
                );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }



        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome To : " + userName;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> GetValue()
        {
            return new string[] { "Value1", "prabath" };
        }
    }
}
