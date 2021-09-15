using System;
using Domain.Habits.Schedules;
using Xunit;

namespace Domain.Tests
{
    public class DaysOfWeekTests
    {
        [Fact]
        public void GetNthFromShouldReturnTheCorrectDay() {
            const DaysOfWeek selection = DaysOfWeek.Monday | DaysOfWeek.Friday | DaysOfWeek.Sunday;
            
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Monday) == 1);
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Friday) == 2);
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Sunday) == 3);
            
            Assert.True(selection.GetNthFrom(DayOfWeek.Tuesday, DayOfWeek.Monday) == 3);
            Assert.True(selection.GetNthFrom(DayOfWeek.Tuesday, DayOfWeek.Friday) == 1);
            Assert.True(selection.GetNthFrom(DayOfWeek.Tuesday, DayOfWeek.Sunday) == 2);
            
            Assert.True(selection.GetNthFrom(DayOfWeek.Friday, DayOfWeek.Monday) == 3);
            Assert.True(selection.GetNthFrom(DayOfWeek.Friday, DayOfWeek.Friday) == 1);
            Assert.True(selection.GetNthFrom(DayOfWeek.Friday, DayOfWeek.Sunday) == 2);
            
            Assert.True(selection.GetNthFrom(DayOfWeek.Saturday, DayOfWeek.Monday) == 2);
            Assert.True(selection.GetNthFrom(DayOfWeek.Saturday, DayOfWeek.Friday) == 3);
            Assert.True(selection.GetNthFrom(DayOfWeek.Saturday, DayOfWeek.Sunday) == 1);
        }

        [Fact]
        public void InvalidDayShouldReturnNull() {
            const DaysOfWeek selection = DaysOfWeek.Monday | DaysOfWeek.Friday | DaysOfWeek.Sunday;
            
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Tuesday) == null);
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Wednesday) == null);
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Thursday) == null);
            Assert.True(selection.GetNthFrom(DayOfWeek.Monday, DayOfWeek.Saturday) == null);
        }
        
    }
}