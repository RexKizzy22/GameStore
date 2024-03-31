using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

namespace GameStore.Api.Middleware;

public static class ErrorHandlingExtensions {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app) {
        app.Run(async context => { 
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionDetails?.Error;

            logger.LogError(
                exception,
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
        });
    }
}