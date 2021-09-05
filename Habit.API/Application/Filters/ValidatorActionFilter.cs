using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Application.Application.Filters
{
    public class ValidatorActionFilter : IActionFilter
    {
        private readonly ILogger<ValidatorActionFilter> _logger;

        public ValidatorActionFilter(ILogger<ValidatorActionFilter> logger) {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context) {
            if (context.ModelState.IsValid)
                return;

            var failures = context.ModelState
                .SelectMany(o =>
                {
                    var (key, value) = o;
                    return value.Errors.Select(err => new ValidationFailure(key, err.ErrorMessage));
                })
                .ToList();

            _logger.LogWarning("Validation errors found for {} {}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path
            );

            throw new ValidationException(failures);
        }

        public void OnActionExecuted(ActionExecutedContext context) {
        }
    }
}