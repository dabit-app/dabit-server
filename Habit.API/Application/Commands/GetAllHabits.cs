using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Application.DTO.Pagination;
using Application.Application.DTO.Paginations;
using Application.Application.DTO.Responses;
using Application.Extensions;
using Domain.Habits.Projections;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Application.Application.Commands
{
    public record GetAllHabitsCommand(int? Page) : IRequest<PaginatedResult<HabitResponse>>, IPaginatedRequest;

    public class GetAllHabitsHandler : IRequestHandler<GetAllHabitsCommand, PaginatedResult<HabitResponse>>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IMongoRepository<HabitProjection> _repository;

        public GetAllHabitsHandler(
            IHttpContextAccessor context,
            IMongoRepository<HabitProjection> repository) {
            _context = context;
            _repository = repository;
        }

        public async Task<PaginatedResult<HabitResponse>> Handle(
            GetAllHabitsCommand request,
            CancellationToken cancellationToken
        ) {
            var filter = Builders<HabitProjection>.Filter.Eq(habit => habit.UserId, _context.DabitUserId());
            var options = new FindOptions<HabitProjection>().PaginateTo(request.Page ?? 1);
            
            var total = await _repository.Collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
            var habits = await _repository.Collection.FindAsync(filter, options, cancellationToken);
            
            return new PaginatedResult<HabitResponse>
            {
                Page = request.Page ?? 1,
                Total = total,
                Items = habits.ToEnumerable(cancellationToken).Select(habit => new HabitResponse(habit))
            };
        }
    }
}