using System.Diagnostics;

namespace GameStore.Api.Middleware;

public class GlobalExceptionMiddleware {
    private readonly RequestDelegate next;

    private readonly ILogger<GlobalExceptionMiddleware> logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, RequestDelegate next)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Could not process request on machine {Machine}. TraceId: {TraceId}",
                Environment.MachineName,
                Activity.Current?.TraceId
            );

            await Results.Problem(
                title: "Something went wrong on our end. We should get it fixed soon",
                statusCode: StatusCodes.Status500InternalServerError,
                extensions: new Dictionary<string, object?>
                {
                    {"traceId", Activity.Current?.TraceId}
                }
            ).ExecuteAsync(context);
        }
    }
}