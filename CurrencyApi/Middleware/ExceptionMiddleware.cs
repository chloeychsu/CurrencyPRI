using System.Net;
using System.Text.Json;
using AutoMapper;

namespace CurrencyApi;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            // Error Message For Http Response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;

            var response = new ExceptionDto(){
                StatusCode = context.Response.StatusCode,
                Message = e.Message.ToString(),
                Details = e.StackTrace.ToString()
            };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
