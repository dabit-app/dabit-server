using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.JWT
{
    public static class Config
    {
        public static AuthenticationBuilder AddDabitJwtBearerConfiguration(
            this AuthenticationBuilder builder,
            IConfiguration configuration
        ) {
            var jwtSecret = configuration.GetSection("JWT:Secret").Value ??
                            throw new Exception("JWT:Secret is required, please add the secret");
            return builder.AddJwtBearer(options => JwtBearerOptions(options, jwtSecret));
        }

        private static void JwtBearerOptions(JwtBearerOptions options, string jwtSecret) {
            options.TokenValidationParameters = DabitJwt.TokenValidationParameters(jwtSecret);
        }
    }
}