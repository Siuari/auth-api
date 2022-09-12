using AuthApi.Application.Dtos;
using AuthApi.Domain.Interfaces.Repositories;
using AuthApi.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Application.CommandsAndQuerys.Commands
{
    public class AuthenticationCommand : IRequest<AuthenticationResponseDto>
    {
        public AuthenticationCommand(AuthenticationRequestDto authenticationRequestDto)
        {
            Email = authenticationRequestDto.Email;
            Password = authenticationRequestDto.Password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationHandler : IRequestHandler<AuthenticationCommand, AuthenticationResponseDto>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;

        public AuthenticationHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, ITokenService tokenService)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public async Task<AuthenticationResponseDto> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmail(request.Email, cancellationToken);

            if (user is null)
            {
                throw new Exception("Fail!! User not valid");
            }

            if (user.PasswordHash != request.Password)
            {
                throw new Exception("Fail!! Password not valid");
            }

            var token = tokenService.GenerateToken(user);
            var refreshToken = tokenService.GenerateRefreshToken(user.Id);

            await refreshTokenRepository.DeleteAsync(user.Id, cancellationToken);
            await refreshTokenRepository.InsertAsync(refreshToken, cancellationToken);
            await refreshTokenRepository.SaveAync(cancellationToken);

            return new AuthenticationResponseDto
            {
                AccessToken = token.Value,
                RefreshToken = refreshToken.Token.Value
            };
        }
    }
}
