using AuthApi.Application.Dtos;
using AuthApi.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Application.CommandsAndQuerys.Commands
{
    public class InsertNewUserCommand : IRequest<Unit>
    {
        public InsertNewUserCommand(InserUserDto userDto)
        {
            Name = userDto.Name;
            Email = userDto.Email;
            Password = userDto.Password;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class InserUserHandler : IRequestHandler<InsertNewUserCommand, Unit>
    {
        private readonly IUserRepository userRepository;

        public InserUserHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Unit> Handle(InsertNewUserCommand request, CancellationToken cancellationToken)
        {
            var user = new IdentityUser
            {
                UserName = request.Name,
                Email = request.Email,
                PasswordHash = request.Password
            };

            await userRepository.InserAsync(user);

            return Unit.Value;
        }
    }
}
