using Domain.SeedWork.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Application.Filters.ErrorHandlers
{
    public class DomainErrorHandler : IErrorHandler<DomainException>
    {
        public void Handle(ExceptionContext context, DomainException exception) {
            var problemDetails = new ProblemDetails
            {
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message,
                Extensions = { { "context", exception.Context.Name } }
            };

            context.Result = new ObjectResult(problemDetails);
            context.HttpContext.Response.StatusCode = (int) problemDetails.Status;
        }
    }
}