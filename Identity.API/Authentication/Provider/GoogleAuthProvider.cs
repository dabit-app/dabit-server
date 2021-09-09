using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Authentication.Provider
{
    public class GoogleAuthProvider : IGoogleAuthProvider
    {
        private const string MetadataAddress = "https://accounts.google.com/.well-known/openid-configuration";
        private const string Issuer = "https://accounts.google.com";
        
        private readonly ILogger<GoogleAuthProvider> _logger;
        private readonly string _audience;

        public GoogleAuthProvider(IConfiguration configuration, ILogger<GoogleAuthProvider> logger) {
            _logger = logger;
            _audience = configuration.GetSection("Provider:Google:Audience").Value ??
                         throw new Exception("Provider:Google:Audience is required, please add it.");
        }

        public async Task<ProviderBaseClaims?> ValidateToken(string token) {
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                MetadataAddress,
                new OpenIdConnectConfigurationRetriever()
            );

            var discoveryDocument = await configurationManager.GetConfigurationAsync();
            var signingKeys = discoveryDocument.SigningKeys;

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = _audience,
                ValidIssuer = Issuer,
                IssuerSigningKeys = signingKeys
            };

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (email != null && id != null)
                    return new ProviderBaseClaims(id, email);

                _logger.LogWarning("Missing email/id from google's claims - this wasn't expected");
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}