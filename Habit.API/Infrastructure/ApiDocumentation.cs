using Habit.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Habit.API.Infrastructure
{
    public static class ApiDocumentation
    {
        public static void AddApiGeneration(this IServiceCollection services) {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Habit.API",
                    Version = "v1",
                    Description = @"
                        Dabit API is the core engine for any Dabit related client UI.
                        Every client application, official or not, use this API to communicate
                        and gather information.
                    ".TrimEachLine()
                });
                
                c.EnableAnnotations();
            });
        }

        public static void UseApiUi(this IApplicationBuilder app) {
            app.UseReDoc(c =>
            {
                c.DocumentTitle = "Dabit API v1";
                c.SpecUrl("/swagger/v1/swagger.json");
                c.ExpandResponses("200,201");
            });
        }
    }
}