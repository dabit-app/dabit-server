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
    public record DeleteHabitCommand(Guid Id) : IRequest;

    public class DeleteHabitHandler : IRequestHandler<DeleteHabitCommand>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public DeleteHabitHandler(
            IHttpContextAccessor context,
            IEventStoreRepository<Habit> habitRepository
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