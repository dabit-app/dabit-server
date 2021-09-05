using System;
using System.Collections.Generic;
using Domain.Habits.Events;
using Domain.Habits.Schedules;
using Domain.SeedWork;

namespace Domain.Habits.Projections
{
    public class HabitProjection : IProjection, IDeletable
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } = default!;

        public Schedule? Schedule { get; set; }

        public SortedSet<int> Completions { get; set; } = new();

        public bool IsDeleted { get; set; }

        public void When(object @event) {
            switch (@event)
            {
                case NewHabitCreated newHabitCreated:
                    Apply(newHabitCreated);
                    return;
                case HabitNameChanged habitNameChanged:
                    Apply(habitNameChanged);
                    return;
                case HabitScheduleDefined habitScheduleDefined:
                    Apply(habitScheduleDefined);
                    return;
                case HabitDayCompleted habitDayCompleted:
                    Apply(habitDayCompleted);
                    return;
                case HabitDayUncompleted habitDayUncompleted:
                    Apply(habitDayUncompleted);
                    return;
                case HabitDeleted habitDeleted:
                    Apply(habitDeleted);
                    break;
                default:
                    return;
            }
        }

        private void Apply(NewHabitCreated @event) {
            Id = @event.Id;
            UserId = @event.UserId;
            Name = @event.Name;
        }

        private void Apply(HabitNameChanged @event) {
            Name = @event.Name;
        }

        private void Apply(HabitScheduleDefined @event) {
            Schedule = @event.Schedule;
        }

        private void Apply(HabitDayCompleted @event) {
            Completions.Add(@event.IntervalNumber);
        }

        private void Apply(HabitDayUncompleted @event) {
            Completions.Remove(@event.IntervalNumber);
        }
        
        private void Apply(HabitDeleted habitDeleted) {
            IsDeleted = true;
        }
    }
}