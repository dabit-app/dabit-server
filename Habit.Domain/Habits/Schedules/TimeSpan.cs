using System;
using System.Collections.Generic;
using Domain.SeedWork.Exceptions;

namespace Domain.Habits.Schedules
{
    /// <summary>
    /// Type to represent a time span >= 1 day
    /// Eg. 1 day, 2 weeks, 15 days
    /// </summary>
    public class TimeSpan : IComparable<TimeSpan>, IEquatable<TimeSpan>
    {
        public TimeUnit Unit { get; }
        public int Count { get; }

        public TimeSpan(TimeUnit unit = TimeUnit.Day, int count = 1) {
            Unit = unit;
            Count = count;

            if (Count <= 0)
                throw new DomainException(typeof(TimeSpan), "Unit cannot be below 1");
        }

        // Comparison

        public int ToDays => Count * Unit switch
        {
            TimeUnit.Day => 1,
            TimeUnit.Week => 7,
            _ => throw new ArgumentOutOfRangeException()
        };

        public int CompareTo(TimeSpan other) {
            var thisDays = ToDays;
            var otherDays = other.ToDays;
            return thisDays.CompareTo(otherDays);
        }

        public static bool operator ==(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) == 0;
        }

        public static bool operator !=(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) != 0;
        }

        public static bool operator <(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(TimeSpan left, TimeSpan right) {
            return Comparer<TimeSpan>.Default.Compare(left, right) >= 0;
        }

        public bool Equals(TimeSpan other) {
            return Comparer<TimeSpan>.Default.Compare(this, other) == 0;
        }

        public override bool Equals(object? obj) {
            return obj is TimeSpan other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked
            {
                return ((int) Unit * 397) ^ Count;
            }
        }
    }
}