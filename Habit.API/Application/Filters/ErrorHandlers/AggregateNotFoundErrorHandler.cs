using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Habit.API.Application.Filters.ErrorHandlers
{
    public class AggregateNotFoundErrorHandler : IErrorHandler<AggregateNotFoundException>
    {
        public void Handle(ExceptionContext context, AggregateNotFoundException exception) {
            var problemDetails = new ProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status404NotFound,
                Detail = exception.Message
            };

            problemDetails.Extensions.Add("id", exception.Id.ToString());

            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int) problemDetails.Status;
        }
    }
}