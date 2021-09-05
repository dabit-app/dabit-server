using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions
{
    public static class UserInfoExtension
    {
        public static Guid DabitUserId(this ClaimsPrincipal claimsPrincipal) {
            var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedAccessException("Missing claim in the JWT token issued");

            return Guid.Parse(claim.Value);
        }

        public static Guid DabitUserId(this IHttpContextAccessor context) {
            return context.HttpContext?.User.DabitUserId() ?? throw new Exception("No http context");
        }
    }
}