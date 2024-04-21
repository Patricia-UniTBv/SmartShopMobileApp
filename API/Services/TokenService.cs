using DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWT(LoggedInUser user, IEnumerable<Claim>? additionalClaims = null);
    }

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
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = GetSecurityKey(configuration)
            };

        public string GenerateJWT(IEnumerable<Claim>? additionalClaims = null)
        {
            var securityKey = GetSecurityKey(_configuration);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expireInMinutes = Convert.ToInt32(_configuration["Jwt:ExpireIMinutes"] ?? "60");

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims!);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],
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
            string jwtKey = _configuration["Jwt:Key"]!;
            if (jwtKey == null)
            {
                // Aruncați o excepție sau tratați în alt mod situația în care cheia nu este definită
                throw new ArgumentNullException("Jwt:Key", "Jwt:Key trebuie să fie definit în configurație.");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }

    }
}
