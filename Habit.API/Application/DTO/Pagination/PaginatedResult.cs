using System.Collections.Generic;

namespace Application.Application.DTO.Pagination
{
    public record PaginatedResult<T>
    {
        public const int PageSize = 100;

        public long Total { get; init; }
        public int Page { get; init; }
        public int Size { get; init; } = PageSize;
        public IEnumerable<T> Items { get; init; } = null!;
    }
}