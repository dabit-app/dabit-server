using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Extensions;
using Domain.Habits;
using Domain.Habits.Schedules;
using Domain.SeedWork;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Application.Commands
{
    public record DefineHabitScheduleCommand(Guid Id, Schedule Schedule) : IRequest;

    public class DefineHabitScheduleHandler : IRequestHandler<DefineHabitScheduleCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public DefineHabitScheduleHandler(
            IHttpContextAccessor httpContextAccessor,
            IEventStoreRepository<Habit> habitRepository
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