using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Application.Filters.ErrorHandlers
{
    public class ValidationErrorHandler : IErrorHandler<ValidationException>
    {
        public void Handle(ExceptionContext context, ValidationException exception) {
            var problemDetails = new ValidationProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validations error happened, please refer to the errors property."
            };

            var errorPerProperty = exception.Errors
                .GroupBy(e => e.PropertyName);

            foreach (var error in errorPerProperty)
            {
                problemDetails.Errors.Add(
                    error.Key,
                    error.Select(e => e.ErrorMessage).ToArray()
                );
            }

            context.Result = new BadRequestObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int) problemDetails.Status;
        }
    }
}