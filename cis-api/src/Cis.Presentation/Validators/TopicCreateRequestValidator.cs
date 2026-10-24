using Cis.Core.Domain.Exceptions;
using Cis.Presentation.Requests;
namespace Cis.Presentation.Validators;

public static class TopicCreateRequestValidator
{
    public static void Validate(TopicCreateRequest? request)
    {
        if (request is null)
        {
            throw new CisValidationException("Request body cannot be null.");
        }
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new CisValidationException("Title is required and cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(request.Description))
        {
            throw new CisValidationException("Description is required and cannot be empty.");
        }
    }
}