using System.Net;
using System.Text.Json;
using Bansi.Examen.Application.Dtos;
using Microsoft.Data.SqlClient;

namespace Bansi.Examen.WebService.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error de base de datos procesando {Path}", context.Request.Path);
            await EscribirRespuesta(context, HttpStatusCode.ServiceUnavailable,
                "No se pudo conectar con la base de datos. Intente más tarde.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado procesando {Path}", context.Request.Path);
            await EscribirRespuesta(context, HttpStatusCode.InternalServerError,
                "Ocurrió un error interno. Intente nuevamente.");
        }
    }

    private static async Task EscribirRespuesta(HttpContext context, HttpStatusCode statusCode, string mensaje)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(ResultadoOperacion.Error(mensaje)));
    }
}
