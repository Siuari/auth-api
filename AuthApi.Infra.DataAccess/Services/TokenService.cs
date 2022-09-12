using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthApi.Domain.Entities;
using AuthApi.Domain.Interfaces.Services;
using AuthApi.Domain.ValueObject;
using AuthApi.Infra.Constantes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Infra.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Token GenerateToken(IdentityUser user)
            => GenerateToken(new Claim[] {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.NameIdentifier, user.UserName),
            });

        public Token GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = configuration["SecretKey"];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new Token(tokenHandler.WriteToken(token));
        }

        public RefreshToken GenerateRefreshToken(string userId)
        {
            var randomKey = new byte[32];
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(randomKey);

            var dateNow = DateTime.UtcNow;

            var token = new RefreshToken
            {
                Created = dateNow,
                UserId = userId,
                Token = new(Convert.ToBase64String(randomKey)),
                Expires = dateNow.AddHours(2),
            };

            return token;
        }

        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
            )
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
