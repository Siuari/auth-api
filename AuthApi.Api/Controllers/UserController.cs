using AuthApi.Application.CommandsAndQuerys.Commands;
using AuthApi.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Insert(
            [FromBody] InserUserDto dto,
            CancellationToken cancellationToken
        )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await mediator.Send(new InsertNewUserCommand(dto), cancellationToken));
        }
    }
}
