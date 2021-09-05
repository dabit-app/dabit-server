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
    public record ChangeHabitNameCommand(Guid Id, string Name) : IRequest;

    public class ChangeHabitNameHandler : IRequestHandler<ChangeHabitNameCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public ChangeHabitNameHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Habit> habitRepository
        ) {
            _context = context;
            _habitRepository = habitRepository;
        }

        public async Task<Unit> Handle(ChangeHabitNameCommand request, CancellationToken cancellationToken) {
            await _habitRepository.GetAndUpdate(
                request.Id,
                habit => habit.ChangeName(request.Name),
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );

            return Unit.Value;
        }
    }
}