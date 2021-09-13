using System;
using Domain.Habits.Schedules;
using Domain.SeedWork.Exceptions;
using Xunit;
using TimeSpan = Domain.Habits.Schedules.TimeSpan;

namespace Domain.Tests
{
    public class ScheduleTests
    {
        [Fact]
        public void ValidSchedule() {
            var everyDay = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(),
                new TimeSpan()
            );

            Assert.Equal(DateTime.Today, everyDay.StartDate);
            Assert.Equal(DateTime.Today.AddMonths(1), everyDay.EndDate);
            Assert.Equal(new TimeSpan(), everyDay.Cadency);
            Assert.Equal(new TimeSpan(), everyDay.Duration);
            Assert.Equal(DaysOfWeek.None, everyDay.DaysOfWeek);

            var everyThreeDay = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(TimeUnit.Day, 3),
                new TimeSpan()
            );

            Assert.Equal(new TimeSpan(TimeUnit.Day, 3), everyThreeDay.Cadency);

            var everyWeekOnTueWed = new Schedule(
                DateTime.Today,
                null,
                new TimeSpan(TimeUnit.Week),
                new TimeSpan(TimeUnit.Week),
                DaysOfWeek.Tuesday | DaysOfWeek.Wednesday
            );

            Assert.Equal(new TimeSpan(TimeUnit.Week), everyWeekOnTueWed.Cadency);
            Assert.Equal(DaysOfWeek.Tuesday | DaysOfWeek.Wednesday, everyWeekOnTueWed.DaysOfWeek);
        }

        [Fact]
        public void CannotEndBeforeTheStart() {
            var scheduleNegativeDate = new Action(() => _ = new Schedule(
                DateTime.Today,
                DateTime.Today.AddDays(-1),
                new TimeSpan(),
                new TimeSpan()
            ));

            var scheduleSameDate = new Action(() => _ = new Schedule(
                DateTime.Today,
                DateTime.Today,
                new TimeSpan(),
                new TimeSpan()
            ));

            Assert.Throws<DomainException>(scheduleNegativeDate);
            Assert.Throws<DomainException>(scheduleSameDate);
        }

        [Fact]
        public void CannotHaveDurationGreaterThanCadency() {
            var schedule1 = new Action(() => _ = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(TimeUnit.Day, 2),
                new TimeSpan(TimeUnit.Day, 3)
            ));

            var schedule2 = new Action(() => _ = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(TimeUnit.Week, 2),
                new TimeSpan(TimeUnit.Day, 15)
            ));

            Assert.Throws<DomainException>(schedule1);
            Assert.Throws<DomainException>(schedule2);
        }

        [Fact]
        public void CannotHaveCadencyLessThanAWeekWithDayOfWeekUsed() {
            var schedule = new Action(() => _ = new Schedule(
                DateTime.Today,
                DateTime.Today.AddMonths(1),
                new TimeSpan(TimeUnit.Day, 2),
                new TimeSpan(TimeUnit.Day, 2),
                DaysOfWeek.Monday | DaysOfWeek.Friday
            ));

            Assert.Throws<DomainException>(schedule);
        }

        [Fact]
        public void OnlyAllowPossibleDays() {
            var schedule = new Schedule(
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(10),
                new TimeSpan(TimeUnit.Day, 3),
                new TimeSpan()
            );

            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today).Item1);
            Assert.True(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(1)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(2)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(3)).Item1);
            Assert.True(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(4)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(5)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(6)).Item1);
            Assert.True(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(7)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(8)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(9)).Item1);
            Assert.False(schedule.ValidateDayCanBeCompleted(DateTime.Today.AddDays(10)).Item1);
        }

        [Fact]
        public void OnlyAllowPossibleDaysInWeek() {
            var baseDate = new DateTime(2021, 08, 29); // sunday
            
            var schedule = new Schedule(
                baseDate.AddDays(1),
                baseDate.AddDays(10),
                new TimeSpan(TimeUnit.Week),
                new TimeSpan(TimeUnit.Week),
                DayOfWeek.Monday | DayOfWeek.Wednesday | DayOfWeek.Saturday | DayOfWeek.Sunday
            );

            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate).Item1); // sun
            Assert.True(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(1)).Item1); // mon
            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(2)).Item1); // tue
            Assert.True(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(3)).Item1); // wed
            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(4)).Item1); // thu
            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(5)).Item1); // fri
            Assert.True(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(6)).Item1); // sat
            Assert.True(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(7)).Item1); // sun
            Assert.True(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(8)).Item1); // mon
            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(9)).Item1); // tue
            Assert.False(schedule.ValidateDayCanBeCompleted(baseDate.AddDays(10)).Item1); // wed
        }
    }
}