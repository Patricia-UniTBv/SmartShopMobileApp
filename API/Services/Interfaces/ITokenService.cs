using DTO;
using System.Security.Claims;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJWT(IEnumerable<Claim>? additionalClaims = null);
        string GenerateJWT(LoggedInUser user, IEnumerable<Claim>? additionalClaims = null);
    }

}
