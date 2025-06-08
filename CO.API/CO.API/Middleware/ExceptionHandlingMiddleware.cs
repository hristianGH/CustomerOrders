using CO.API.Exceptions;
using System.Net;
using System.Text.Json;

namespace CO.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                if (!context.Request.Headers.ContainsKey("UserName"))
                {
                    context.Request.Headers.Append("UserName", "default_user");
                }
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred.");

                int statusCode = (int)HttpStatusCode.InternalServerError;
                string message = "An unexpected error occurred.";

                switch (ex)
                {
                    // only process expected exceptions here
                    case ApiException apiEx:
                        statusCode = apiEx.StatusCode;
                        message = apiEx.Message;
                        break;
                }

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message,
                    traceId = context.TraceIdentifier
                };

                string json = JsonSerializer.Serialize(response);

                logger.LogError($"ExceptionHandlingMiddleware: Error Response: {json}");

                await context.Response.WriteAsync(json);
            }
        }
    }
}
