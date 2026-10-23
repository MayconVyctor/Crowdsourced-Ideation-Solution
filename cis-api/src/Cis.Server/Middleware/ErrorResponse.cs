namespace Cis.Server.Middleware;

public record ErrorResponse(int Status, string Error, string Message);
