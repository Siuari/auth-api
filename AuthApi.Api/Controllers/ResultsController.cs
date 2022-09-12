using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        [Authorize]
        [HttpGet("authorized")]
        public IActionResult GetAuthorizedResult()
            => Ok(new { message = "Consult authorized!" });

        [AllowAnonymous]
        [HttpGet("anonymous")]
        public IActionResult GetAnonymowsResult()
            => Ok(new { message = "Consult allowed anonymously!" });

        [Authorize(Roles = "")]
        [HttpGet("authorized/with-roles")]
        public IActionResult GetAuthorizedWithRolesResult()
            => Ok(new { message = "Consult allowed because the user has roles!" });
    }
}
