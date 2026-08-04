using System.Net.Http.Json;
using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Interfaces;

namespace Bansi.Examen.Infrastructure.Gateways;

public class HttpExamenGateway : IExamenGateway
{
    private readonly HttpClient _httpClient;

    public HttpExamenGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ResultadoOperacion> AgregarAsync(ExamenDto examen) =>
        EnviarAsync(() => _httpClient.PostAsJsonAsync("api/examen", examen));

    public Task<ResultadoOperacion> ActualizarAsync(ExamenDto examen) =>
        EnviarAsync(() => _httpClient.PutAsJsonAsync("api/examen", examen));

    public Task<ResultadoOperacion> EliminarAsync(int id) =>
        EnviarAsync(() => _httpClient.DeleteAsync($"api/examen/{id}"));

    public async Task<ResultadoConsulta> ConsultarAsync(string? nombre, string? descripcion)
    {
        try
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(nombre))
                query.Add($"nombre={Uri.EscapeDataString(nombre)}");
            if (!string.IsNullOrWhiteSpace(descripcion))
                query.Add($"descripcion={Uri.EscapeDataString(descripcion)}");

            var url = "api/examen" + (query.Count > 0 ? "?" + string.Join("&", query) : string.Empty);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return ResultadoConsulta.Error($"El WebService respondió {(int)response.StatusCode}");

            var resultado = await response.Content.ReadFromJsonAsync<ResultadoConsulta>();
            return resultado ?? ResultadoConsulta.Error("Respuesta vacía del WebService");
        }
        catch (Exception ex)
        {
            return ResultadoConsulta.Error(ex.Message);
        }
    }

    private static async Task<ResultadoOperacion> EnviarAsync(Func<Task<HttpResponseMessage>> enviar)
    {
        try
        {
            var response = await enviar();

            if (!response.IsSuccessStatusCode)
                return ResultadoOperacion.Error($"El WebService respondió {(int)response.StatusCode}");

            var resultado = await response.Content.ReadFromJsonAsync<ResultadoOperacion>();
            return resultado ?? ResultadoOperacion.Error("Respuesta vacía del WebService");
        }
        catch (Exception ex)
        {
            return ResultadoOperacion.Error(ex.Message);
        }
    }
}
