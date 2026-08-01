using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinanceTracker.Api.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new UnauthorizedAccessException("Token does not contain a user id.");
        return Guid.Parse(sub);
    }
}
