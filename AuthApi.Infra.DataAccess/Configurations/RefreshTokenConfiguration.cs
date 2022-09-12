using AuthApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Infra.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken), "dbo");

            builder.Property(x => x.Id);
            builder.Property(x => x.Created);
            builder.Property(x => x.Expires);
            builder.Property(x => x.Revoked);
            builder.OwnsOne(x => x.Token).Property(x => x.Value);
            builder.Property(x => x.UserId);

            builder.HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<RefreshToken>(x => x.UserId);
        }
    }
}
