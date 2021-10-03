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
    public record DeleteHabitCommand(Guid Id) : IRequest;

    public class DeleteHabitHandler : IRequestHandler<DeleteHabitCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Domain.Habits.Habit> _habitRepository;

        public DeleteHabitHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Domain.Habits.Habit> habitRepository
        ) {
            _context = context;
            _habitRepository = habitRepository;
        }

        public async Task<Unit> Handle(DeleteHabitCommand request, CancellationToken cancellationToken) {
            await _habitRepository.GetAndUpdate(
                request.Id,
                habit => habit.Delete(),
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );
            return Unit.Value;
        }
    }
}