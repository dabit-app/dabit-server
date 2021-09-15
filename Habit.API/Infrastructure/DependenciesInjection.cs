using System;
using Application.Application.Behaviors;
using Application.Application.Commands;
using Application.Application.DTO.Pagination;
using Application.Application.DTO.Requests;
using Application.Application.DTO.Responses;
using Application.Application.Filters.ErrorHandlers;
using Application.Application.Notifiers;
using Application.Application.Validations;
using Application.Hubs;
using Domain.Habits;
using Domain.Habits.Events;
using Domain.Habits.Projections;
using Domain.SeedWork;
using Domain.SeedWork.Exceptions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Exceptions;
using Infrastructure.Projections;
using Infrastructure.Repositories;
using Infrastructure.Subscriptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

// @formatter:max_line_length 180
namespace Application.Infrastructure
{
    public static class DependenciesInjection
    {
        public static void AddApplicationDependenciesInjection(this IServiceCollection services) {
            // repositories
            services.AddScoped<IEventStoreRepository<Habit>, EventStoreRepository<Habit>>();
            services.AddCheckpointRepository("subscription-checkpoint");
            services.AddMongoRepository<HabitProjection>("habits", collection => { collection.AddGuidIndex(habit => habit.UserId); });

            // commands
            services.AddTransient<IRequestHandler<CreateHabitCommand, Habit>, CreateHabitHandler>();
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

            // projections
            services.Project<NewHabitCreated, HabitProjection>();
            services.Project<HabitNameChanged, HabitProjection>();
            services.Project<HabitScheduleDefined, HabitProjection>();
            services.Project<HabitEventCompleted, HabitProjection>();
            services.Project<HabitEventUncompleted, HabitProjection>();
            services.Project<HabitDeleted, HabitProjection>();

            // signalR notifiers
            services.AddTransient<IHabitsHubNotifier<ChangeHabitNameCommand>, ChangeHabitsNameNotifier>();

            // behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HabitNotifyBehavior<,>));

            // miscellaneous
            AddAggregateMapper(services);
        }

        private static void AddAggregateMapper(IServiceCollection services) {
            services.AddSingleton<IAggregateMapper>(_ =>
            {
                return new AggregateMapper(new[]
                {
                    typeof(Habit)
                });
            });
        }
    }
}