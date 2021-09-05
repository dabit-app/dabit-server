using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Habits;
using Domain.SeedWork;
using MediatR;

namespace Application.Application.Commands
{
    public record CreateHabitCommand(Guid Id, Guid UserId, string Name) : IRequest<Habit>;

    public class CreateHabitHandler : IRequestHandler<CreateHabitCommand, Habit>
    {
        private readonly IEventStoreRepository<Habit> _habitRepository;

        public CreateHabitHandler(IEventStoreRepository<Habit> habitRepository) {
            _habitRepository = habitRepository;
        }

        public async Task<Habit> Handle(CreateHabitCommand command, CancellationToken cancellationToken) {
            var habit = new Habit(command.Id, command.UserId, command.Name);
            await _habitRepository.Store(habit, cancellationToken);
            return habit;
        }
    }
}