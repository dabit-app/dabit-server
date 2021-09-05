using Application.Application.DTO.Paginations;

namespace Application.Application.DTO.Requests
{
    public record GetAllHabitsRequest : IPaginatedRequest
    {
        public int? Page { get; init; }
    }
}