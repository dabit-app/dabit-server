using Identity.API.Authentication.Provider;
using Identity.API.Models;
using Identity.API.Repository;
using Identity.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Infrastructure
{
    public static class DependenciesInjection
    {
        public static void AddApplicationDependenciesInjection(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            // db
            services.AddMongoDbContext(configuration);
            services.AddMongoCollection<User>("user", async collection =>
            {
                await collection.AddHashIndex(o => o.GoogleId!);
            });

            // repo
            services.AddScoped<UserRepository>();

            // auth
            services.AddTransient<IDabitJwt, DabitJwt>();
            services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
        }
    }
}