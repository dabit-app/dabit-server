using System;

namespace Domain.Habits.Events
{
    public class NewHabitCreated
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        
        public NewHabitCreated(Guid id, Guid userId, string name) {
            Id = id;
            UserId = userId;
            Name = name;
        }
    }
}