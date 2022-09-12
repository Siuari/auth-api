using AuthApi.Application.Dtos;
using AuthApi.Domain.Entities;
using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Domain.Interfaces.Services;
using AuthApi.Domain.ValueObject;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Application.CommandsAndQuerys.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>
    {
        public RefreshTokenCommand(Token accessToken, Token oldRefreshToken)
        {
            OldRefreshToken = oldRefreshToken;
            AccessToken = accessToken;
        }

        public Token AccessToken { get; set; }
        public Token OldRefreshToken { get; set; }
    }

    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly ITokenService tokenService;

        public RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.tokenService = tokenService;
        }

        public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var claimsPrincipal = tokenService.GetClaimsFromToken(request.AccessToken.Value);

            var userId = claimsPrincipal.Identity.Name;

            var oldRefreshToken = await GetOldRefreshToken(
                userId,
                request,
                cancellationToken
            );

            if (!oldRefreshToken.IsActive)
            {
                throw new Exception("Invalid token!");
            }

            await refreshTokenRepository.DeleteAsync(oldRefreshToken);

            var accessToken = tokenService.GenerateToken(claimsPrincipal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken(userId);

            await refreshTokenRepository.InsertAsync(newRefreshToken, cancellationToken);
            await refreshTokenRepository.SaveAync(cancellationToken);

            return new RefreshTokenResponseDto()
            {
                AccessToken = accessToken.Value,
                RefreshToken = newRefreshToken.Token.Value
            };
        }

        private async Task<RefreshToken> GetOldRefreshToken(
            string userId,
            RefreshTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var oldRefreshToken = await refreshTokenRepository.GetByUserIdAsync(
                userId,
                cancellationToken
            );

            if (oldRefreshToken.Token.Value != request.OldRefreshToken.Value)
            {
                throw new Exception("Refresh token doesn't exist!");
            }

            if (oldRefreshToken.IsRevoked || oldRefreshToken.IsExpired)
            {
                throw new Exception("Refresh token is not valid anymore.");
            }

            return oldRefreshToken;
        }
    }
}
