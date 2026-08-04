using Bansi.Examen.Application.Dtos;

namespace Bansi.Examen.Application.Interfaces;

public interface IExamenGateway
{
    Task<ResultadoOperacion> AgregarAsync(ExamenDto examen);
    Task<ResultadoOperacion> ActualizarAsync(ExamenDto examen);
    Task<ResultadoOperacion> EliminarAsync(int id);
    Task<ResultadoConsulta> ConsultarAsync(string? nombre, string? descripcion);
}
