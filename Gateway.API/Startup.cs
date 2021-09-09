using Identity.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gateway.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            var proxyBuilder = services.AddReverseProxy();
            proxyBuilder.LoadFromConfig(_configuration.GetSection("ReverseProxy"));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDabitJwtBearerConfiguration(_configuration);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .WithHeaders("content-type")
                );
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("dabit-jwt", policy => policy.RequireAuthenticatedUser());
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapReverseProxy(); });
        }
    }
}