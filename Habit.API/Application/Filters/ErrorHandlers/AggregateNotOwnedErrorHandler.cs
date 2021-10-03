using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Habit.API.Application.Filters.ErrorHandlers
{
    public class AggregateNotOwnedErrorHandler : IErrorHandler<AggregateNotOwnedException>
    {
        public void Handle(ExceptionContext context, AggregateNotOwnedException exception) {
            var problemDetails = new ProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status401Unauthorized,
                Detail = "You do not own this resource"
            };

            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int) problemDetails.Status;
        }
    }
}