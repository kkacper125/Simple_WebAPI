using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Primitives;

namespace Simple_WebAPI.Middlewares;

public class InputSanitizationMiddleware
{
    private readonly RequestDelegate _next; 

    public InputSanitizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sanitizedQuery = context.Request.Query
            .ToDictionary(kvp => kvp.Key, kvp => new StringValues(HtmlEncoder.Default.Encode(kvp.Value!)));

        context.Request.Query = new QueryCollection(sanitizedQuery);

        if (context.Request.HasFormContentType)
        {
            var sanitizedForm = context.Request.Form
                .ToDictionary(kvp => kvp.Key, kvp => new StringValues(HtmlEncoder.Default.Encode(kvp.Value!)));

            context.Request.Form = new FormCollection(sanitizedForm);
        }

        if(context.Request.HasJsonContentType())
        {
            var body = await context.Request.ReadFromJsonAsync<Dictionary<string, object>>();

            if (body != null)
            {
                Console.WriteLine("Body not null");
                body = SanitizeJson(body);
            }

            var serializedBody = JsonSerializer.Serialize(body);
            var serializedStream = new MemoryStream(Encoding.UTF8.GetBytes(serializedBody));
            context.Request.Body = serializedStream;
            context.Request.ContentLength = serializedStream.Length;
        }

        await _next(context);
    } 

    private Dictionary<string, object> SanitizeJson(Dictionary<string, object> json)
    {
        var sanitized = new Dictionary<string, object>();

        foreach (var kvp in json)
        {
            if (kvp.Value is JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.String:
                        sanitized[kvp.Key] = HtmlEncoder.Default.Encode(element.GetString()!);
                        break;
                    case JsonValueKind.Object:
                        sanitized[kvp.Key] = SanitizeJson(JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText())!);
                        break;
                    case JsonValueKind.Array:
                        sanitized[kvp.Key] = element.EnumerateArray()
                            .Select(
                                e => 
                                e.ValueKind == JsonValueKind.String ? (object)HtmlEncoder.Default.Encode(e.GetString()!) : 
                                e.ValueKind == JsonValueKind.Object ? (object)SanitizeJson(JsonSerializer.Deserialize<Dictionary<string, object>>(e.GetRawText())!) 
                                : (object)e)
                            .ToList();
                        break;
                    default:
                        sanitized[kvp.Key] = kvp.Value;
                        break;
                }
            }
            else
            {
                sanitized[kvp.Key] = kvp.Value;
            }
        }

        return sanitized;
    }
}
