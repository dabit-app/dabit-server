using System.Collections.Generic;
using System.Collections.Immutable;
using Domain.SeedWork.Exceptions;

namespace Domain.Habits.Completions
{
    public class HabitCompletions
    {
        public IReadOnlyCollection<int> Values => _completions.ToImmutableHashSet();

        private readonly HashSet<int> _completions = new();

        public void Insert(int nthEvent) {
            if (_completions.Contains(nthEvent))
                throw new DomainException(typeof(HabitCompletions), "Cannot add an event that is already counted");

            _completions.Add(nthEvent);
        }

        public void Remove(int nthEvent) {
            if (!_completions.Contains(nthEvent))
                throw new DomainException(typeof(HabitCompletions), "Cannot remove an event that was never counted");

            _completions.Remove(nthEvent);
        }
    }
}