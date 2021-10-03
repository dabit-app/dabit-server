using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Habit.API.Application.Filters.ErrorHandlers
{
    public interface IErrorHandler<T> where T : Exception
    {
        void Handle(ExceptionContext context, T exception);
    }
}