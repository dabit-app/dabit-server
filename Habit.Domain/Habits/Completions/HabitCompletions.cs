using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Application.Extensions;
using Domain.Habits.Schedules;
using Domain.SeedWork.Exceptions;

namespace Domain.Habits.Completions
{
    public class HabitCompletions
    {
        public IReadOnlyCollection<int> Values => _completions.ToImmutableHashSet();

        private readonly HashSet<int> _completions = new();
        private readonly Schedule _schedule;

        public HabitCompletions(Schedule schedule) {
            _schedule = schedule;
        }

        public void ThrowIfInvalidDay(DateTime day) {
            var (canBeCompleted, reason) = _schedule.ValidateDayCanBeCompleted(day);
            if (!canBeCompleted)
            {
                throw new DomainException(
                    typeof(HabitCompletions),
                    $"{day.ToShortDate()} can not be done on that day, because the {reason}"
                );
            }

            var index = _schedule.GetIntervalNumberForDay(day);

            if (_completions.Contains(index))
                throw new DomainException(typeof(HabitCompletions), "Cannot add a day that is already counted");
        }

        public void Insert(int intervalNumber) {
            if (_completions.Contains(intervalNumber))
                throw new DomainException(typeof(HabitCompletions), "Cannot add an existing interval");
            
            _completions.Add(intervalNumber);
        }

        public void Remove(int intervalNumber) {
            if (!_completions.Contains(intervalNumber))
                throw new DomainException(typeof(HabitCompletions), "Cannot remove a day that was never counted");

            _completions.Remove(intervalNumber);
        }
    }
}