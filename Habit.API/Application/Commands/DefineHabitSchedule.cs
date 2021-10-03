using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Habits.Schedules;
using Domain.SeedWork;
using Habit.API.Extensions;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Habit.API.Application.Commands
{
    public record DefineHabitScheduleCommand(Guid Id, Schedule Schedule) : IRequest;

    public class DefineHabitScheduleHandler : IRequestHandler<DefineHabitScheduleCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Domain.Habits.Habit> _habitRepository;

        public DefineHabitScheduleHandler(
            IHttpContextAccessor httpContextAccessor,
            IEventStoreRepository<Domain.Habits.Habit> habitRepository
        ) {
            _context = httpContextAccessor;
            _habitRepository = habitRepository;
        }

        public async Task<Unit> Handle(DefineHabitScheduleCommand request, CancellationToken cancellationToken) {
            await _habitRepository.GetAndUpdate(
                request.Id,
                habit => habit.DefineSchedule(request.Schedule),
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}