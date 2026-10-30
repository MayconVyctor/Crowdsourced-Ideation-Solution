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
public class TopicCommandController(ITopicCommandService topicCommandService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TopicResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TopicResponse>> CreateTopicAsync(
        [FromHeader(Name = "X-User-Id")] long? userId,
        [FromBody] TopicCreateRequest request)
    {
        if (userId is null)
        {
            throw new CisValidationException("X-User-Id header is required.");
        }

        TopicCreateRequestValidator.Validate(request);

        var topic = await topicCommandService.CreateTopicAsync(request.Title!, request.Description!, (ulong)userId.Value);
        var response = TopicPresentationMapper.ToResponse(topic);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
