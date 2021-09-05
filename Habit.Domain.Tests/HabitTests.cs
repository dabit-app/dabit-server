using System;
using Domain.Habits;
using Domain.Habits.Schedules;
using Domain.SeedWork.Exceptions;
using Xunit;
using TimeSpan = Domain.Habits.Schedules.TimeSpan;

namespace Domain.Tests
{
    public class HabitTests
    {
        [Fact]
        public void StandardScenario() {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var name = "foo";

            var habit = new Habit(id, userId, name);

            Assert.Equal(id, habit.Id);
            Assert.Equal(userId, habit.UserId);
            Assert.Equal(name, habit.Name);
            Assert.Equal(1, habit.Version);
            Assert.Null(habit.Schedule);

            name = "bar";
            habit.ChangeName(name);

            Assert.Equal(name, habit.Name);
            Assert.Equal(2, habit.Version);

            var schedule = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(),
                new TimeSpan()
            );

            habit.DefineSchedule(schedule);

            Assert.Equal(schedule, habit.Schedule);
            Assert.Equal(3, habit.Version);

            var events = habit.DequeueUncommittedEvents();
            Assert.Equal(3, events.Length);

            name = "foo bar";
            habit.ChangeName(name);

            Assert.Equal(name, habit.Name);
            Assert.Equal(4, habit.Version);

            events = habit.DequeueUncommittedEvents();
            Assert.Single(events);
        }

        [Fact]
        public void ScheduleShouldBeImmutable() {
            var habit = new Habit(Guid.NewGuid(), Guid.NewGuid(), "foo");

            var scheduleA = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(),
                new TimeSpan()
            );

            var scheduleB = new Schedule(
                DateTime.Today.AddMonths(1),
                DateTime.Today.AddMonths(2),
                new TimeSpan(TimeUnit.Week),
                new TimeSpan(TimeUnit.Week)
            );

            habit.DefineSchedule(scheduleA);
            Assert.Throws<DomainException>(() => habit.DefineSchedule(scheduleB));
            Assert.Equal(scheduleA, habit.Schedule);
        }
    }
}