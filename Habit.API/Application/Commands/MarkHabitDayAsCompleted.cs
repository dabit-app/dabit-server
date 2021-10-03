using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using Habit.API.Extensions;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Habit.API.Application.Commands
{
    public record MarkHabitDayAsCompletedCommand(Guid HabitId, DateTime Day) : IRequest;

    public class MarkHabitDayAsCompletedHandler : IRequestHandler<MarkHabitDayAsCompletedCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Domain.Habits.Habit> _habitRepository;

        public MarkHabitDayAsCompletedHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Domain.Habits.Habit> habitRepository
        ) {
            _context = context;
            _habitRepository = habitRepository;
        }

        public async Task<Unit> Handle(MarkHabitDayAsCompletedCommand request, CancellationToken cancellationToken) {
            await _habitRepository.GetAndUpdate(
                request.HabitId,
                habit => habit.MarkDayAsCompleted(request.Day),
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}