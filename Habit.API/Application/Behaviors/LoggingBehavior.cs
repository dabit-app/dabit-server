using System.Threading;
using System.Threading.Tasks;
using Habit.API.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Habit.API.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        ) {
            var commandTypeName = request!.GetGenericTypeName();

            _logger.LogInformation($"Handling command {commandTypeName} {request}");
            var response = await next();
            _logger.LogInformation($"Command {commandTypeName} handled");

            return response;
        }
    }
}