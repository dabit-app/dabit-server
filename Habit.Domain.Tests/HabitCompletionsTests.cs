using System;
using System.Linq;
using Domain.Habits.Completions;
using Domain.Habits.Schedules;
using Domain.SeedWork.Exceptions;
using Xunit;
using DayOfWeek = Domain.Habits.Schedules.DayOfWeek;
using TimeSpan = Domain.Habits.Schedules.TimeSpan;

namespace Domain.Tests
{
    public class HabitCompletionsTests
    {
        [Fact]
        public void EverydayCompletionShouldAcceptEveryday() {
            var everydaySchedule = new Schedule(
                DateTime.Today,
                DateTime.Today.AddDays(10),
                new TimeSpan(),
                new TimeSpan()
            );

            var completions = new HabitCompletions(everydaySchedule);

            foreach (var index in Enumerable.Range(0, 10))
            {
                var date = DateTime.Today.AddDays(index);
                var intervalNumber = everydaySchedule.GetIntervalNumberForDay(date);
                completions.Insert(intervalNumber);
            }

            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(-1))); // out of bound
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(10))); // out of bound
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(1))); // duplicate

            Assert.True(completions.Values.Count == 10);
        }

        [Fact]
        public void EveryThreeDayShouldOnlyAcceptOne() {
            // every 5 days, there's 3 days to do a task once.
            var schedule = new Schedule(
                DateTime.Today,
                DateTime.Today.AddDays(14),
                new TimeSpan(TimeUnit.Day, 5),
                new TimeSpan(TimeUnit.Day, 3)
            );

            var completions = new HabitCompletions(schedule);

            completions.Insert(schedule.GetIntervalNumberForDay(DateTime.Today));

            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(1)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(2)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(3)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(4))); // end of the first cycle

            // 5, 6, 7 are OK, 8, 9 NOK
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(8)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(DateTime.Today.AddDays(9)));

            completions.Insert(schedule.GetIntervalNumberForDay(DateTime.Today.AddDays(7))); // should be inserted as 5
            Assert.Contains(5, completions.Values);
        }

        [Fact]
        public void UsingDayOfWeekItShouldAcceptCorrectDays() {
            var baseDate = new DateTime(2021, 08, 27); // friday
            
            var schedule = new Schedule(
                baseDate,
                baseDate.AddDays(14),
                new TimeSpan(TimeUnit.Week),
                new TimeSpan(TimeUnit.Week),
                DayOfWeek.Monday | DayOfWeek.Wednesday | DayOfWeek.Friday | DayOfWeek.Sunday
            );

            var completions = new HabitCompletions(schedule);
            
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(2))); // week 1, sun
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(3))); // week 1, mon
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(5))); // week 1, wed
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(7))); // week 2, fri
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(9))); // week 2, sun
            completions.Insert(schedule.GetIntervalNumberForDay(baseDate.AddDays(10))); // week 2, mon
            
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(baseDate.AddDays(4)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(baseDate.AddDays(8)));
            Assert.Throws<DomainException>(() => completions.ThrowIfInvalidDay(baseDate.AddDays(11)));

            Assert.True(completions.Values.Count == 6);
            
        }
    }
}