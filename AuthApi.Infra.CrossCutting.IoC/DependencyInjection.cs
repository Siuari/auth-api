using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Domain.Interfaces.Services;
using AuthApi.Infra.Context;
using AuthApi.Infra.Repositories;
using AuthApi.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi.Infra.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddDbContext<AuthContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }
    }
}