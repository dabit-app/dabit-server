using System;
using System.Threading.Tasks;
using Habit.API.Application.Commands;
using Habit.API.Application.DTO.Pagination;
using Habit.API.Application.DTO.Requests;
using Habit.API.Application.DTO.Responses;
using Habit.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Habit.API.Controllers
{
    [Route("api/[controller]")]
    [SwaggerTag("Create and manage habits.")]
    public class HabitController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HabitController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet(Name = "Get all habits of the current user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<PaginatedResult<HabitResponse>> GetAllHabits([FromQuery] GetAllHabitsRequest request) {
            var command = new GetAllHabitsCommand(request.Page);
            return await _mediator.Send(command);
        }

        [HttpGet("{id:guid}", Name = "Get a single habit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<HabitResponse> GetHabit([FromRoute] Guid id) {
            var command = new GetHabitCommand(id);
            return await _mediator.Send(command);
        }

        [HttpPost(Name = "Create a habit")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> NewHabit([FromBody] CreateHabitRequest request) {
            var id = Guid.NewGuid();
            var command = new CreateHabitCommand(id, User.DabitUserId(), request.Name);
            var habit = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetHabit), new {id}, new HabitResponse(habit));
        }

        [HttpPatch("{id:guid}/name", Name = "Change the name of a habit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> ChangeName([FromRoute] Guid id, [FromBody] ChangeHabitNameRequest request) {
            var command = new ChangeHabitNameCommand(id, request.Name);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id:guid}/schedule", Name = "Define the schedule of a habit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DecideSchedule(
            [FromRoute] Guid id,
            [FromBody] DefineHabitScheduleRequest request
        ) {
            var command = new DefineHabitScheduleCommand(id, request.Schedule.ToSchedule());
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{id:guid}/completed", Name = "Mark a day as done")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> MarkDayAsCompleted(
            [FromRoute] Guid id,
            [FromBody] MarkHabitDayCompleteness request
        ) {
            var command = new MarkHabitDayAsCompletedCommand(id, request.Day);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}/completed", Name = "Remove the completion mark on a given day")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> MarkDayAsUncompleted(
            [FromRoute] Guid id,
            [FromBody] MarkHabitDayCompleteness request
        ) {
            var command = new MarkHabitDayAsUncompletedCommand(id, request.Day);
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}", Name = "Delete an habit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteHabit([FromRoute] Guid id) {
            var command = new DeleteHabitCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}