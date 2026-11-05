using System.Globalization;
using Cis.Core.Application.Services;
using Cis.Core.Domain.Exceptions;
using Cis.Presentation.Mappers;
using Cis.Presentation.Requests;
using Cis.Presentation.Responses;
using Cis.Presentation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cis.Presentation.Controllers;

[ApiController]
[Route("api/v1/topics")]
public class IdeaCommandController(
    IIdeaCommandService ideaCommandService) : ControllerBase
{
    [HttpPost("{id}/ideas")]
    [ProducesResponseType(typeof(IdeaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<IdeaResponse>> CreateIdeaAsync(
        [FromRoute] ulong id,
        [FromBody] IdeaCreateRequest request,
        [FromHeader(Name = "X-User-Id")] string? userIdHeader)
    {
        IdeaCreateRequestValidator.Validate(request);

        if (!ulong.TryParse(
            userIdHeader,
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out var userId))
        {
            throw new CisValidationException(
                "X-User-Id header is required and must contain a valid user ID.");
        }

        var idea = await ideaCommandService.CreateIdeaAsync(
            id,
            request.Title!,
            request.Description!,
            userId);

        var response = IdeaResponseMapper.ToResponse(idea);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
