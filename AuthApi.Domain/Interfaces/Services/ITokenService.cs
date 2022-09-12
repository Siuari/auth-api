using AuthApi.Domain.Entities;
using AuthApi.Domain.ValueObject;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthApi.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Token GenerateToken(IdentityUser user);
        Token GenerateToken(IEnumerable<Claim> claims);
        RefreshToken GenerateRefreshToken(string ipAddress);
        ClaimsPrincipal GetClaimsFromToken(string token);
    }
}
