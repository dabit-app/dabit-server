using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using MediatR;

namespace Habit.API.Application.Commands
{
    public record CreateHabitCommand(Guid Id, Guid UserId, string Name) : IRequest<Domain.Habits.Habit>;

    public class CreateHabitHandler : IRequestHandler<CreateHabitCommand, Domain.Habits.Habit>
    {
        private readonly IEventStoreRepository<Domain.Habits.Habit> _habitRepository;

        public CreateHabitHandler(IEventStoreRepository<Domain.Habits.Habit> habitRepository) {
            _habitRepository = habitRepository;
        }

        public async Task<Domain.Habits.Habit> Handle(CreateHabitCommand command, CancellationToken cancellationToken) {
            var habit = new Domain.Habits.Habit(command.Id, command.UserId, command.Name);
            await _habitRepository.Store(habit, cancellationToken);
            return habit;
        }
    }
}