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
        public void CannotInsertADateOutOfBound() {
            var schedule = new Schedule(
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(10),
                new TimeSpan(),
                new TimeSpan()
            );

            // out of bound should not be possible
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(DateTime.Today));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(DateTime.Today.AddDays(10)));

            // but within it shouldn't throw any exception
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(1)) == 1);
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(9)) == 9);
        }

        [Fact]
        public void OnlyValidDayShouldBeAccepted() {
            // every 3 day we got 2 day to achieve something
            var schedule = new Schedule(
                DateTime.Today.AddDays(1),
                DateTime.Today.AddDays(10),
                new TimeSpan(TimeUnit.Day, 3),
                new TimeSpan(TimeUnit.Day, 2)
            );

            // day: 1st 2nd 3rd 4th 5th 6th 7th 8th 9th ...
            // nth: [1] [1] [-] [2] [2] [-] [3] [3] [-] [4]

            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(1)) == 1);
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(2)) == 1);
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(DateTime.Today.AddDays(3)));
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(4)) == 2);
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(5)) == 2);
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(DateTime.Today.AddDays(6)));
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(7)) == 3);
            Assert.True(schedule.GetNthEventAt(DateTime.Today.AddDays(8)) == 3);
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(DateTime.Today.AddDays(9)));
        }

        [Fact]
        public void OnlyAllowPossibleDaysInWeek() {
            var baseDate = new DateTime(2021, 08, 29); // sunday

            var schedule = new Schedule(
                baseDate.AddDays(1),
                baseDate.AddDays(10),
                new TimeSpan(TimeUnit.Week),
                new TimeSpan(TimeUnit.Week),
                DaysOfWeek.Monday | DaysOfWeek.Wednesday | DaysOfWeek.Saturday | DaysOfWeek.Sunday
            );

            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(1)) == 1); // mon
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(2))); // tue
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(3)) == 2); // wed
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(4))); // thu
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(5))); // fri
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(6)) == 3); // sat
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(7)) == 4); // sun
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(8)) == 5); // mon
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(9))); // tue
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(10))); // wed
        }

        [Fact]
        public void OnTwoWeekIntervalSecondWeekShouldBeEmpty() {
            var baseDate = new DateTime(2021, 08, 29); // sunday
            var schedule = new Schedule(
                baseDate.AddDays(1),
                baseDate.AddDays(16), // 2 week + 1 day
                new TimeSpan(TimeUnit.Week, 2),
                new TimeSpan(TimeUnit.Week),
                DaysOfWeek.Monday | DaysOfWeek.Tuesday | DaysOfWeek.Wednesday | DaysOfWeek.Thursday
                | DaysOfWeek.Friday | DaysOfWeek.Saturday | DaysOfWeek.Sunday
            );

            // first week should be counted - according to the schedule, all days are enabled
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(1)) == 1);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(2)) == 2);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(3)) == 3);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(4)) == 4);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(5)) == 5);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(6)) == 6);
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(7)) == 7);

            // this week should be empty
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(8)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(9)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(10)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(11)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(12)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(13)));
            Assert.Throws<DomainException>(() => schedule.GetNthEventAt(baseDate.AddDays(14)));
            
            // then third week should be again all good
            Assert.True(schedule.GetNthEventAt(baseDate.AddDays(15)) == 8);
        }
    }
}