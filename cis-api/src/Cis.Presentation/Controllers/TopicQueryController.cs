using Cis.Core.Application.Services;
using Cis.Presentation.Mappers;
using Cis.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cis.Presentation.Controllers;

[ApiController]
[Route("api/v1/topics")]
public class TopicQueryController(ITopicQueryService topicQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TopicResponse>>> ReadTopicsAsync()
    {
        var topics = await topicQueryService.ReadTopicsAsync();

        var responses = topics
            .Select(TopicResponseMapper.ToResponse)
            .ToList();

        return Ok(responses);
    }
}
