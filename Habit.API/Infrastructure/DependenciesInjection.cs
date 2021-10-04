using System;
using Domain.SeedWork.Exceptions;
using FluentValidation;
using Habit.API.Application.Behaviors;
using Habit.API.Application.Commands;
using Habit.API.Application.DTO.Pagination;
using Habit.API.Application.DTO.Requests;
using Habit.API.Application.DTO.Responses;
using Habit.API.Application.Filters.ErrorHandlers;
using Habit.API.Application.Notifiers;
using Habit.API.Application.Validations;
using Habit.API.Hubs;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// @formatter:max_line_length 180
namespace Habit.API.Infrastructure
{
    public static class DependenciesInjection
    {
        public static void AddApplicationDependenciesInjection(this IServiceCollection services) {
            // commands
            services.AddTransient<IRequestHandler<CreateHabitCommand, Domain.Habits.Habit>, CreateHabitHandler>();
            services.AddTransient<IRequestHandler<ChangeHabitNameCommand>, ChangeHabitNameHandler>();
            services.AddTransient<IRequestHandler<DefineHabitScheduleCommand>, DefineHabitScheduleHandler>();
            services.AddTransient<IRequestHandler<MarkHabitDayAsCompletedCommand>, MarkHabitDayAsCompletedHandler>();
            services.AddTransient<IRequestHandler<MarkHabitDayAsUncompletedCommand>, MarkHabitDayAsUncompletedHandler>();
            services.AddTransient<IRequestHandler<GetAllHabitsCommand, PaginatedResult<HabitResponse>>, GetAllHabitsHandler>();
            services.AddTransient<IRequestHandler<GetHabitCommand, HabitResponse>, GetHabitHandler>();

            // validators
            services.AddScoped<IValidator<CreateHabitRequest>, CreateHabitRequestValidator>();
            services.AddScoped<IValidator<ChangeHabitNameRequest>, ChangeHabitNameRequestValidator>();
            services.AddScoped<IValidator<DefineHabitScheduleRequest>, DefineHabitScheduleRequestValidator>();
            services.AddScoped<IValidator<MarkHabitDayCompleteness>, MarkHabitDayCompletenessValidator>();
            services.AddScoped<IValidator<GetAllHabitsRequest>, GetAllHabitsRequestValidator>();

            // errors handler
            services.AddTransient<IErrorHandler<Exception>, UnknownErrorHandler>();
            services.AddTransient<IErrorHandler<DomainException>, DomainErrorHandler>();
            services.AddTransient<IErrorHandler<ValidationException>, ValidationErrorHandler>();
            services.AddTransient<IErrorHandler<AggregateNotFoundException>, AggregateNotFoundErrorHandler>();
            services.AddTransient<IErrorHandler<AggregateNotOwnedException>, AggregateNotOwnedErrorHandler>();

            // signalR notifiers
            services.AddTransient<IHabitsHubNotifier<ChangeHabitNameCommand>, ChangeHabitsNameNotifier>();

            // behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HabitNotifyBehavior<,>));

        }
    }
}