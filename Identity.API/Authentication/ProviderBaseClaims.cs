namespace Identity.API.Authentication.Provider
{
    public record ProviderBaseClaims
    {
        public ProviderBaseClaims(string id, string email) {
            Id = id;
            Email = email;
        }

        public string Id { get; init; }
        public string Email { get; init; }
    }
}