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
    public record MarkHabitDayAsCompletedCommand(Guid HabitId, DateTime Day) : IRequest;

    public class MarkHabitDayAsCompletedHandler : IRequestHandler<MarkHabitDayAsCompletedCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public MarkHabitDayAsCompletedHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Habit> habitRepository
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