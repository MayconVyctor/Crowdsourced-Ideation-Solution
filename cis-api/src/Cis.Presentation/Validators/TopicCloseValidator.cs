using System.Globalization;
using Cis.Core.Domain.Exceptions;

namespace Cis.Presentation.Validators;

public static class TopicCloseValidator
{
    public static ulong Validate(string? userIdHeader)
    {
        if (string.IsNullOrWhiteSpace(userIdHeader) ||
            !ulong.TryParse(userIdHeader, NumberStyles.None, CultureInfo.InvariantCulture, out var userId) ||
            userId == 0)
        {
            throw new CisValidationException("X-User-Id header is required and must contain a valid user ID.");
        }

        return userId;
    }
}

