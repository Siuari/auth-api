using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task InserAsync(IdentityUser user);

        Task<IdentityUser> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        );

        Task<IdentityUser> GetByEmail(string email, CancellationToken cancellationToken);

        Task<bool> ValidatePassword(IdentityUser user, string password);
    }
}
