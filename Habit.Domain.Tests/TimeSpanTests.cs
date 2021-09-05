using System;
using Domain.Habits.Schedules;
using Domain.SeedWork.Exceptions;
using Xunit;
using TimeSpan = Domain.Habits.Schedules.TimeSpan;

namespace Domain.Tests
{
    public class TimeSpanTests
    {
        [Fact]
        public void ValidTimeSpan() {
            var twoDays = new TimeSpan(TimeUnit.Day, 2);
            var oneWeek = new TimeSpan(TimeUnit.Week, 1);

            Assert.Equal(TimeUnit.Day, twoDays.Unit);
            Assert.Equal(2, twoDays.Count);

            Assert.Equal(TimeUnit.Week, oneWeek.Unit);
            Assert.Equal(1, oneWeek.Count);
        }

        [Fact]
        public void InvalidTimeSpan() {
            var zeroDay = new Action(() => _ = new TimeSpan(TimeUnit.Day, 0));
            var negativeDay = new Action(() => _ = new TimeSpan(TimeUnit.Day, -1));

            Assert.Throws<DomainException>(zeroDay);
            Assert.Throws<DomainException>(negativeDay);
        }

        [Fact]
        public void ValidCompare() {
            var oneDay = new TimeSpan();
            var sixDays = new TimeSpan(TimeUnit.Day, 6);
            var sevenDays = new TimeSpan(TimeUnit.Day, 7);
            var eightDays = new TimeSpan(TimeUnit.Day, 8);
            var oneWeek = new TimeSpan(TimeUnit.Week, 1);
            var twoWeek = new TimeSpan(TimeUnit.Week, 2);
            var anotherOneDay = new TimeSpan();
            var anotherOneWeek = new TimeSpan(TimeUnit.Week);

            Assert.True(oneDay < sevenDays);
            Assert.True(sevenDays > oneDay);

            Assert.True(oneDay < oneWeek);
            Assert.True(eightDays > oneWeek);
            Assert.True(twoWeek > oneWeek);

            Assert.True(eightDays >= sevenDays);
            Assert.True(oneWeek >= sevenDays);
            Assert.False(sixDays >= sevenDays);

            Assert.False(eightDays <= sevenDays);
            Assert.True(oneWeek <= sevenDays);
            Assert.True(sixDays <= sevenDays);

            Assert.True(sevenDays == oneWeek);
            Assert.True(oneDay != oneWeek);
            Assert.True(eightDays != oneWeek);

            Assert.True(anotherOneDay == oneDay);
            Assert.True(anotherOneWeek == oneWeek);
        }
    }
}