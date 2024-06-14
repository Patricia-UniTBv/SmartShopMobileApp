using API.Services.Interfaces;
using DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) =>
            new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:7157",
                IssuerSigningKey = GetSecurityKey(configuration)
            };

        public string GenerateJWT(IEnumerable<Claim>? additionalClaims = null)
        {
            var securityKey = GetSecurityKey(_configuration);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expireInMinutes = 60;

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims!);

            var issuer = "https://localhost:7157"; 
       

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: "*",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireInMinutes),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJWT(LoggedInUser user, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new(ClaimTypes.GivenName, user.FirstName),
                    new(ClaimTypes.Surname, user.LastName),
                    new(ClaimTypes.Email, user.Email),
                };
            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims!);

            return GenerateJWT(claims);
        }

        private static SymmetricSecurityKey GetSecurityKey(IConfiguration _configuration)
        {
            string jwtKey = "@SomERandomStromgKey/(@bH6)-@-#-$";

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }

    }
}
