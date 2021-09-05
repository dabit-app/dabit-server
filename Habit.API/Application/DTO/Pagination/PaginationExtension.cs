using MongoDB.Driver;

namespace Application.Application.DTO.Pagination
{
    public static class PaginationExtension
    {
        public static FindOptions<T> PaginateTo<T>(this FindOptions<T> findOptions, int page) {
            findOptions.Skip = PaginatedResult<T>.PageSize * (page - 1);
            findOptions.Limit = PaginatedResult<T>.PageSize;
            return findOptions;
        }
    }
}