using System.Threading.Tasks;

namespace Identity.API.Authentication.Provider
{
    public interface IAuthProvider
    {
        public Task<ProviderBaseClaims?> ValidateToken(string token);
    }
}