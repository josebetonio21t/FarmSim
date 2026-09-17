namespace FarmSim.Api.Infrastructure;

public sealed class RequestCorrelationMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var supplied)
            && !string.IsNullOrWhiteSpace(supplied)
                ? supplied.ToString()
                : context.TraceIdentifier;

        context.Response.Headers[HeaderName] = correlationId;
        await next(context);
    }
}
