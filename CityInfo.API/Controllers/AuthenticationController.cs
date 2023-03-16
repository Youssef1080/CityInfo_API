using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICityInfoRepository cityInfo;
        private readonly IMapper mapper;
        private readonly IConfiguration config;

        public class AuthenticationRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }

            public AuthenticationRequestBody(string userName, string password)
            {
                UserName = userName;
                Password = password;
            }
        }

        public AuthenticationController(ICityInfoRepository cityInfo, IMapper mapper, IConfiguration config)
        {
            this.cityInfo = cityInfo;
            this.mapper = mapper;
            this.config = config;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticatedUser)
        {
            // Step 1 => Validate the username/password
            var user = await ValidateUserCredentials(authenticatedUser.UserName, authenticatedUser.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Step 2 => Create a Token(securityKey, signCredentials, claims, JWT)
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(config["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // information on who the user is value-pairs called claims
            var claimsForToken = new List<Claim>()
            {
                new Claim("sub", $"{user.Id}"),
                new Claim("given-name", $"{user.FirstName}"),
                new Claim("family-name", $"{user.LastName}"),
                new Claim("city", user.City),
            };

            // using JWT for security
            var jwtSecurityToken = new JwtSecurityToken
                (config["Authentication:Issuer"], config["Authentication:Audience"],
                claimsForToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1), signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private async Task<CityInfoUser> ValidateUserCredentials(string? userName, string? password)
        {
            var entity = await cityInfo.AuthenticateUser(userName, password);

            return mapper.Map<CityInfoUser>(entity);
        }
    }
}