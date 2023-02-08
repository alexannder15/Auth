using Auth.Application.Exceptions;
using Auth.Domain.Dtos.Responses;
using System.Net;
using System.Text.Json;

namespace Auth.WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(
        ILogger<ErrorHandlerMiddleware> logger,
        RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            string message = "Something was wrong!";
            string errorCode = string.Empty;

            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            switch (error)
            {
                case EmailNotFoundException:
                    errorCode = "400.02.001";
                    message = error.Message;
                    break;

                case EmailAlreadyExistException:
                    errorCode = "400.02.002";
                    message = error.Message;
                    break;

                case InvalidCredentialsException:
                    errorCode = "400.02.004";
                    message = error.Message;
                    break;

                case UnhandledException:
                    errorCode = "500.02.001";
                    message = error.Message;
                    break;

                default:
                    errorCode = "500.02.999";
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            response.StatusCode = (int)statusCode;

            _logger.LogInformation("An error has ocurred: {0}", error.Message);
            var result = JsonSerializer.Serialize(CreateResponseErrorModel(errorCode, message));
            await response.WriteAsync(result);
        }
    }

    private static ResponseError CreateResponseErrorModel(string errorCode, string message)
    {
        return new ResponseError
        {
            Code = errorCode,
            DateTime = DateTimeOffset.Now,
            Mesage = message
        };
    }
}
