using AuthApi.Application.CommandsAndQuerys.Commands;
using AuthApi.Application.Dtos;
using AuthApi.Domain.ValueObject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        public AuthenticationController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate(
            [FromBody] AuthenticationRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await mediator.Send(new AuthenticationCommand(requestDto), cancellationToken);

            Response.Cookies.Append(
                "refresh-token",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(2)
                });

            return Ok(result.AccessToken);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["refresh-token"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { message = "Refresh token is required" });
            }

            var result = await mediator.Send(
                new RefreshTokenCommand(
                    new Token(refreshToken),
                    new Token(refreshToken)
                ),
                cancellationToken
            );

            Response.Cookies.Append(
                "refresh-token",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(2)
                });

            return Ok(result.AccessToken);
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> RevokeToken(string userId, CancellationToken cancellationToken)
            => Ok(await mediator.Send(new RevokeRefreshTokenCommand(userId), cancellationToken));
        
        [AllowAnonymous]
        [HttpGet("secret")]
        public async Task<IActionResult> ObterSecret()
        {
            var secretKey = configuration["SecretKey"];

            return Ok(secretKey);
        }
    }
}
