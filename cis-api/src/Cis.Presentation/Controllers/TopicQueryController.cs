using Cis.Presentation.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cis.Presentation.Controllers;

[ApiController]
[Route("api/v1/topics")]
public class TopicQueryController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<TopicResponse>> ReadTopicsAsync()
    {
        return Ok(new List<TopicResponse>());
    }
}
