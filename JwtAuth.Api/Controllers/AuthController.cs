using JwtAuth.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IConfiguration configuration, ITokenService tokenService)
        {
            Configuration = configuration;
            TokenService = tokenService;
        }

        public IConfiguration Configuration { get; }
        public ITokenService TokenService { get; }

        [AllowAnonymous]
        [HttpPost("GetToken")]
        public IActionResult GetToken([FromBody] AuthModel authModel)
        {
            string password= Configuration[authModel.UserName] ?? string.Empty;

            if(authModel.Password == null)
            {
                return new UnauthorizedResult();
            }

            if(!authModel.Password.Equals(password))
            {
                return this.StatusCode(StatusCodes.Status401Unauthorized, "Invalid Credentials");
            }

            return Ok(TokenService.GenerateJwtToken(authModel.UserName));
        }

        public class AuthModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}
