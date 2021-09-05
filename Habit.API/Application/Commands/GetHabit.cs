using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Application.DTO.Responses;
using Application.Extensions;
using Domain.Habits.Projections;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Application.Commands
{
    public record GetHabitCommand(Guid Id) : IRequest<HabitResponse>;

    public class GetHabitHandler : IRequestHandler<GetHabitCommand, HabitResponse>
    {
        private readonly IHttpContextAccessor _context;
        private readonly IMongoRepository<HabitProjection> _repository;

        public GetHabitHandler(
            IHttpContextAccessor context,
            IMongoRepository<HabitProjection> repository) {
            _context = context;
            _repository = repository;
        }

        public async Task<HabitResponse> Handle(GetHabitCommand request, CancellationToken cancellationToken) {
            var projection = await _repository.Get(
                request.Id,
                habit => habit.UserId == _context.DabitUserId(),
                cancellationToken
            );

            return new HabitResponse(projection);
        }
    }
}