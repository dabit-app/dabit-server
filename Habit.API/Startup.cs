using FluentValidation.AspNetCore;
using Habit.API.Application.Filters;
using Habit.API.Hubs;
using Habit.API.Infrastructure;
using Identity.JWT;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Habit.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            // context
            services.AddHttpContextAccessor();

            // infrastructure
            services.AddEventStoreDbContext(_configuration);
            services.AddMongoDbContext(_configuration);

            // application
            services.AddMediatR(typeof(Startup));
            services.AddInfrastructureDependenciesInjection();
            services.AddApplicationDependenciesInjection();
            services.AddFluentValidation();
            services.AddSignalR();
            
            // cors
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .WithHeaders("content-type", "authorization")
                );
            });

            // auth
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDabitJwtBearerConfiguration(_configuration);

            // api
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.Filters.Add(typeof(ValidatorActionFilter));
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            // api doc
            services.AddApiGeneration();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseApiUi();
            }

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<HabitsHub>("/realtime");
            });
        }
    }
}