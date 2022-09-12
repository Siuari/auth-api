using AuthApi.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Application.CommandsAndQuerys.Commands
{
    public class RevokeRefreshTokenCommand : IRequest
    {
        public string UserId { get; set; }

        public RevokeRefreshTokenCommand(string userId)
        {
            UserId = userId;
        }
    }

    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public RevokeRefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await refreshTokenRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                throw new Exception("User doesn't have a refresh token!");
            }

            if(refreshToken.IsActive || refreshToken.IsExpired)
            {
                throw new Exception("User refresh token is alread deactivated or expired!");
            }

            refreshToken.Revoked = DateTime.UtcNow;

            await refreshTokenRepository.SaveAync(cancellationToken);

            return Unit.Value;
        }
    }
}
