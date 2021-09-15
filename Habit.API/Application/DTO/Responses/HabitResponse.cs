using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Application.Application.DTO.Shared;
using Domain.Habits;
using Domain.Habits.Projections;

namespace Application.Application.DTO.Responses
{
    public record HabitResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public ScheduleResponse? Schedule { get; set; }
        
        public IEnumerable<int> Completions { get; set; }

        public HabitResponse(HabitProjection projection) {
            Id = projection.Id;
            Name = projection.Name;
            Schedule = projection.Schedule == null ? null : ScheduleResponse.From(projection.Schedule);
            Completions = projection.Completions;
        }

        public HabitResponse(Habit habit) {
            Id = habit.Id;
            Name = habit.Name;
            Schedule = habit.Schedule == null ? null : ScheduleResponse.From(habit.Schedule);
            Completions = habit.Completions?.Values ?? new List<int>();
        }
    }
}