using AuthApi.Domain.Entities;
using AuthApi.Domain.ValueObject;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Domain.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task InsertAsync(
            RefreshToken refreshToken, 
            CancellationToken cancellationToken = default
        );

        Task<RefreshToken> GetByUserIdAsync(
            string userId, 
            CancellationToken cancellationToken = default
        );

        Task<RefreshToken?> GetByRefreshTokenAsync(
            Token refreshToken,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(
            string userId,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(RefreshToken refreshToken);

        Task<int> SaveAync(CancellationToken cancellationToken = default);
    }
}
