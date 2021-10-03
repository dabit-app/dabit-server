using Habit.API.Application.DTO.Pagination;

namespace Habit.API.Application.DTO.Requests
{
    public record GetAllHabitsRequest : IPaginatedRequest
    {
        public int? Page { get; init; }
    }
}