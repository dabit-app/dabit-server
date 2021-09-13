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
                case HabitEventCompleted habitDayCompleted:
                    Apply(habitDayCompleted);
                    return;
                case HabitEventUncompleted habitDayUncompleted:
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

        private void Apply(HabitEventCompleted @event) {
            Completions.Add(@event.NthEvent);
        }

        private void Apply(HabitEventUncompleted @event) {
            Completions.Remove(@event.NthEvent);
        }
        
        private void Apply(HabitDeleted habitDeleted) {
            IsDeleted = true;
        }
    }
}