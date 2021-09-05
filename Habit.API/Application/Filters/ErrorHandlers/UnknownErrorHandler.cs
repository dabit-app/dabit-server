using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Application.Filters.ErrorHandlers
{
    public class UnknownErrorHandler : IErrorHandler<Exception>
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public UnknownErrorHandler(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger) {
            _env = env;
            _logger = logger;
        }

        public void Handle(ExceptionContext context, Exception exception) {
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            var response = new JsonErrorResponse
            {
                Messages = new[] {"An error occured, please try again later."}
            };

            if (_env.IsDevelopment())
            {
                response.DeveloperMessage = context.Exception;
            }

            context.Result = new InternalServerErrorObjectResult(response);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        private class JsonErrorResponse
        {
            public string[] Messages { get; init; }
            public object DeveloperMessage { get; set; }
        }

        private class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object error)
                : base(error) {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}