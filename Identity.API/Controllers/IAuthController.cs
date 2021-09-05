using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    public interface IAuthController
    {
        Task<ActionResult<AuthResponse>> Register([FromBody] AuthRequest request);
        Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request);
    }

    public record AuthRequest(string Token);

    public record AuthResponse(string Token);
}