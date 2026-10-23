using Cis.Core.Application.Services;
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
    public async Task<ActionResult<TopicResponse>> CreateTopicAsync([FromBody] TopicCreateRequest request)
    {
        TopicCreateRequestValidator.Validate(request);

        // Usuário padrão 1 (sistema) conforme o contrato inicial desta etapa
        const ulong defaultUserId = 1;

        var topic = await topicCommandService.CreateTopicAsync(request.Title!, request.Description!, defaultUserId);
        var response = TopicPresentationMapper.ToResponse(topic);

        return StatusCode(StatusCodes.Status201Created, response);
    }
}