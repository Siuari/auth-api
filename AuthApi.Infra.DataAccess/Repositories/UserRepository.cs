using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Infra.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityUser> GetByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        )
            => await userManager.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        public async Task InserAsync(IdentityUser user)
            => await userManager.CreateAsync(user);


        public async Task<IdentityUser> GetByEmail(string email, CancellationToken cancellationToken)
            => await userManager.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        public Task<bool> ValidatePassword(IdentityUser user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
