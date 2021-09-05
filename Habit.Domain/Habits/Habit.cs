using System;
using Domain.Habits.Completions;
using Domain.Habits.Events;
using Domain.Habits.Schedules;
using Domain.SeedWork;
using Domain.SeedWork.Exceptions;

namespace Domain.Habits
{
    public class Habit : Aggregate
    {
        public Guid UserId { get; private set; }

        public string Name { get; private set; } = null!;

        public Schedule? Schedule { get; private set; }

        public HabitCompletions? Completions { get; private set; }

        // Action

        public Habit(Guid id, Guid userId, string name) {
            var @event = new NewHabitCreated(id, userId, name);
            Enqueue(@event);
            Apply(@event);
        }

        public void ChangeName(string name) {
            CheckIfNotDeleted();

            var @event = new HabitNameChanged(name);
            Enqueue(@event);
            Apply(@event);
        }

        public void DefineSchedule(Schedule schedule) {
            CheckIfNotDeleted();
            
            if (Schedule != null)
                throw new DomainException(typeof(Habit), "You cannot set a schedule a second time, this is immutable.");

            var @event = new HabitScheduleDefined(schedule);
            Enqueue(@event);
            Apply(@event);
        }

        public void MarkDayAsCompleted(DateTime day) {
            CheckIfNotDeleted();
            
            if (Completions == null || Schedule == null)
                throw new DomainException(typeof(Habit), "You must define a schedule first");

            var @event = new HabitDayCompleted(day, Schedule);
            Enqueue(@event);
            Apply(@event);
        }

        public void MarkDayAsUncompleted(DateTime day) {
            CheckIfNotDeleted();
            
            if (Completions == null || Schedule == null)
                throw new DomainException(typeof(Habit), "You must define a schedule first");

            var @event = new HabitDayUncompleted(day, Schedule);
            Enqueue(@event);
            Apply(@event);
        }

        public void Delete() {
            CheckIfNotDeleted();
            
            var @event = new HabitDeleted();
            Enqueue(@event);
            Apply(@event);
        }

        // When

        public override void When(object @event) {
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
                    return;
            }
        }

        // Apply

        private void Apply(NewHabitCreated @event) {
            Version++;
            Id = @event.Id;
            UserId = @event.UserId;
            Name = @event.Name;
        }

        private void Apply(HabitNameChanged @event) {
            Version++;
            Name = @event.Name;
        }

        private void Apply(HabitScheduleDefined @event) {
            Version++;
            Schedule = @event.Schedule;
            Completions = new HabitCompletions(@event.Schedule);
        }

        private void Apply(HabitDayCompleted @event) {
            Version++;
            Completions?.Insert(@event.IntervalNumber);
        }

        private void Apply(HabitDayUncompleted @event) {
            Version++;
            Completions?.Remove(@event.IntervalNumber);
        }

        private void Apply(HabitDeleted @event) {
            Version++;
            IsDeleted = true;
        }

        // Stuff

        // ReSharper disable once UnusedMember.Local
        private Habit() {
            // required for Activator
        }

        private void CheckIfNotDeleted() {
            if (IsDeleted)
                throw new DomainException(typeof(Habit), "Cannot change a deleted event");
        }
    }
}