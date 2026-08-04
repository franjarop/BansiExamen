using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Services;

namespace Bansi.Examen.AccesoDatos;

public class ClsExamen
{
    private readonly ExamenService _examenService;

    public ClsExamen(ExamenService examenService)
    {
        _examenService = examenService;
    }

    public Task<ResultadoOperacion> AgregarExamen(ExamenDto examen) => _examenService.AgregarExamen(examen);

    public Task<ResultadoOperacion> ActualizarExamen(ExamenDto examen) => _examenService.ActualizarExamen(examen);

    public Task<ResultadoOperacion> EliminarExamen(int id) => _examenService.EliminarExamen(id);

    public Task<ResultadoConsulta> ConsultarExamen(string? nombre, string? descripcion) => _examenService.ConsultarExamen(nombre, descripcion);
}
