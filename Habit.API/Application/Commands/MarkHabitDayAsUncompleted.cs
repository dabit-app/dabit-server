using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Extensions;
using Domain.Habits;
using Domain.SeedWork;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Application.Commands
{
    public record MarkHabitDayAsUncompletedCommand(Guid HabitId, DateTime Day) : IRequest;

    public class MarkHabitDayAsUncompletedHandler : IRequestHandler<MarkHabitDayAsUncompletedCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public MarkHabitDayAsUncompletedHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Habit> habitRepository
        ) {
            _context = context;
            _habitRepository = habitRepository;
        }

        public async Task<Unit> Handle(MarkHabitDayAsUncompletedCommand request, CancellationToken cancellationToken) {
            await _habitRepository.GetAndUpdate(
                request.HabitId,
                habit => habit.MarkDayAsUncompleted(request.Day),
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}