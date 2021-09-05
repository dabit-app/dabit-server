using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Application.Filters.ErrorHandlers
{
    public interface IErrorHandler<T> where T : Exception
    {
        void Handle(ExceptionContext context, T exception);
    }
}