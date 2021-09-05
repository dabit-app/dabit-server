using System.Threading.Tasks;
using Identity.API.Authentication;
using Identity.API.Authentication.Provider;
using Identity.API.Models;
using Identity.API.Repository;
using Identity.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Identity.API.Controllers
{
    [Route("api/auth/google")]
    [SwaggerTag("User authentication")]
    public class GoogleAuthController : ControllerBase, IAuthController
    {
        private readonly IGoogleAuthProvider _googleAuthProvider;
        private readonly IDabitJwt _dabitJwt;
        private readonly UserRepository _userRepository;

        public GoogleAuthController(
            IGoogleAuthProvider googleAuthProvider,
            IDabitJwt dabitJwt,
            UserRepository userRepository
        ) {
            _googleAuthProvider = googleAuthProvider;
            _dabitJwt = dabitJwt;
            _userRepository = userRepository;
        }

        [HttpPost("register", Name = "Register using google as auth provider")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest request) {
            var claims = await _googleAuthProvider.ValidateToken(request.Token);

            if (claims == null)
                return Problem(statusCode: StatusCodes.Status401Unauthorized);

            var existingUser = await _userRepository.FindUserFromGoogleId(claims.Id);
            if (existingUser != null)
                return Problem(
                    "This google account is already linked to an account",
                    statusCode: StatusCodes.Status400BadRequest
                );

            var user = new User {Email = claims.Email, GoogleId = claims.Id};
            await _userRepository.CreateNewUser(user);

            var token = _dabitJwt.GenerateToken(user.Id);

            Response.StatusCode = StatusCodes.Status201Created;
            return new AuthResponse(token);
        }

        [HttpPost("login", Name = "Login using google as auth provider")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request) {
            var claims = await _googleAuthProvider.ValidateToken(request.Token);

            if (claims == null)
                return Problem(statusCode: StatusCodes.Status401Unauthorized);

            var existingUser = await _userRepository.FindUserFromGoogleId(claims.Id);
            if (existingUser == null)
                return Problem(
                    "This google account does not have an account here",
                    statusCode: StatusCodes.Status400BadRequest
                );

            var token = _dabitJwt.GenerateToken(existingUser.Id);

            Response.StatusCode = StatusCodes.Status200OK;
            return new AuthResponse(token);
        }
    }
}