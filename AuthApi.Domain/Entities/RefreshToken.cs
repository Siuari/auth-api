using AuthApi.Domain.ValueObject;
using System;

namespace AuthApi.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }
        public Token Token { get; set; }
        public bool IsRevoked { get => Revoked.HasValue; }
        public bool IsExpired { get => DateTime.Now > Expires; }
        public bool IsActive { get => !IsExpired && !IsRevoked; }
    }
}
