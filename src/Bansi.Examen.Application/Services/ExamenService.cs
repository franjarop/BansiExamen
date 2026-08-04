using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Interfaces;
using Bansi.Examen.Application.Validation;

namespace Bansi.Examen.Application.Services;

public class ExamenService
{
    private readonly IExamenGateway _gateway;

    public ExamenService(IExamenGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<ResultadoOperacion> AgregarExamen(ExamenDto examen)
    {
        var error = ExamenValidator.ValidarDatos(examen.Nombre, examen.Descripcion);
        if (error is not null)
            return ResultadoOperacion.Error(error);

        return await _gateway.AgregarAsync(examen);
    }

    public async Task<ResultadoOperacion> ActualizarExamen(ExamenDto examen)
    {
        var errorId = ExamenValidator.ValidarId(examen.Id);
        if (errorId is not null)
            return ResultadoOperacion.Error(errorId);

        var errorDatos = ExamenValidator.ValidarDatos(examen.Nombre, examen.Descripcion);
        if (errorDatos is not null)
            return ResultadoOperacion.Error(errorDatos);

        return await _gateway.ActualizarAsync(examen);
    }

    public async Task<ResultadoOperacion> EliminarExamen(int id)
    {
        var error = ExamenValidator.ValidarId(id);
        if (error is not null)
            return ResultadoOperacion.Error(error);

        return await _gateway.EliminarAsync(id);
    }

    public async Task<ResultadoConsulta> ConsultarExamen(string? nombre, string? descripcion)
    {
        return await _gateway.ConsultarAsync(nombre, descripcion);
    }
}
