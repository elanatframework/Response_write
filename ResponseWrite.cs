using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

public static class ResponseWriteExtensions
{
    public static void Write(this HttpResponse response, string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        if (response.HttpContext.Items.ContainsKey("Elanat.ResponseWrite"))
            response.HttpContext.Items["Elanat.ResponseWrite"] += text;
        else
            response.HttpContext.Items["Elanat.ResponseWrite"] = text;
    }
}

public class ResponseWriteMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseWriteMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (context.Items.ContainsKey("Elanat.ResponseWrite"))
        {
            var text = context.Items["Elanat.ResponseWrite"]?.ToString();
            if (!string.IsNullOrEmpty(text))
                await context.Response.WriteAsync(text);
        }
    }
}

public static class ResponseWriteMiddlewareExtensions
{
    public static IApplicationBuilder ResponseWrite(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseWriteMiddleware>();
    }
}