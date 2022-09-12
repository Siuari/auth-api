using AuthApi.Application.CommandsAndQuerys.Commands;
using AuthApi.Domain.Interfaces.Repositories;
using Moq;

namespace AuthApi.Tests.Handlers
{
    public class AuthenticationHandlerTests
    {
        private readonly AuthenticationHandler handler;
        private readonly Mock<IRefreshTokenRepository> refreshTokenRepository;
        private readonly Mock<IUserRepository> userRepository;

        public AuthenticationHandlerTests()
        {
            refreshTokenRepository = new();
            userRepository = new();
            
            handler = new(
                refreshTokenRepository.Object,
                userRepository.Object
            );
        }

        private static void Arrange()
        {

        }
    }
}
