using System;
using Domain.SeedWork.Exceptions;
using FluentValidation;
using Habit.API.Application.Filters.ErrorHandlers;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Habit.API.Application.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public HttpGlobalExceptionFilter(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void OnException(ExceptionContext context) {
            switch (context.Exception)
            {
                case ValidationException exception:
                    ResolveFor(context, exception);
                    break;
                
                case DomainException exception:
                    ResolveFor(context, exception);
                    break;
                
                case AggregateNotFoundException exception:
                    ResolveFor(context, exception);
                    break;
                
                case AggregateNotOwnedException exception:
                    ResolveFor(context, exception);
                    break;

                default:
                    ResolveFor(context, context.Exception);
                    break;
            }
        }

        private void ResolveFor<T>(ExceptionContext context, T exception) where T : Exception {
            var handler = _serviceProvider.GetService<IErrorHandler<T>>();
            handler?.Handle(context, exception);
        }
    }
}