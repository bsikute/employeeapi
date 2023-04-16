using EmployeeApi.Models;

namespace EmployeeApi.Extensions;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"Somethig went wrong: {ex.ToString()}");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = 500;

            await response.WriteAsync(new ErrorDetails
            {
                StatusCode = response.StatusCode,
                Message = "Something went wrong"
            }.ToString()
             );
        }
    }
}