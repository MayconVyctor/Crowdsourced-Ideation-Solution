namespace Cis.Core.Domain.Exceptions;

public class CisValidationException : Exception
{
    public CisValidationException(string message) : base(message)
    {

    }

    public CisValidationException(string message, Exception innerException) : base(message, innerException)
    {

    }
}