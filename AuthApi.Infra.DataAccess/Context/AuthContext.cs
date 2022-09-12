using AuthApi.Domain.Entities;
using AuthApi.Infra.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infra.Context
{
    public class AuthContext : IdentityDbContext
    {
        public AuthContext()
        { }

        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        { }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
