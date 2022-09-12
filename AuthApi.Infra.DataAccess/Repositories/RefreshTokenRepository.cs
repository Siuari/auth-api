using AuthApi.Domain.Entities;
using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Domain.ValueObject;
using AuthApi.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Infra.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AuthContext authContext;

        public RefreshTokenRepository(AuthContext authContext)
        {
            this.authContext = authContext;
        }

        public async Task DeleteAsync(
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            var refreshToken = await authContext.Set<RefreshToken>()
                .FirstAsync(x => x.UserId == userId, cancellationToken);

            if (refreshToken == null)
            {
                return;
            }

            authContext.Set<RefreshToken>()
                .Remove(refreshToken);
        }

        public async Task DeleteAsync(RefreshToken refreshToken)
            => authContext.Set<RefreshToken>().Remove(refreshToken);

        public async Task<RefreshToken> GetByRefreshTokenAsync(Token refreshToken, CancellationToken cancellationToken = default)
            => await authContext.Set<RefreshToken>()
                .FirstOrDefaultAsync(x => x.Token.Value == refreshToken.Value, cancellationToken);

        public async Task<RefreshToken> GetByUserIdAsync(
            string userId, 
            CancellationToken cancellationToken = default
        )
            => await authContext.Set<RefreshToken>()
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        public async Task InsertAsync(
            RefreshToken refreshToken,
            CancellationToken cancellationToken = default
        )
            => await authContext.AddAsync(
                refreshToken, 
                cancellationToken
            );

        public async Task<int> SaveAync(CancellationToken cancellationToken = default)
            => await authContext.SaveChangesAsync(cancellationToken);
    }
}
