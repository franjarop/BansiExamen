using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bansi.Examen.WebService.Controllers;

[ApiController]
[Route("api/examen")]
public class ExamenController : ControllerBase
{
    private readonly ExamenService _examenService;

    public ExamenController(ExamenService examenService)
    {
        _examenService = examenService;
    }

    [HttpPost]
    public async Task<ActionResult<ResultadoOperacion>> Agregar(ExamenDto examen)
    {
        var resultado = await _examenService.AgregarExamen(examen);
        return Ok(resultado);
    }

    [HttpPut]
    public async Task<ActionResult<ResultadoOperacion>> Actualizar(ExamenDto examen)
    {
        var resultado = await _examenService.ActualizarExamen(examen);
        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResultadoOperacion>> Eliminar(int id)
    {
        var resultado = await _examenService.EliminarExamen(id);
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<ActionResult<ResultadoConsulta>> Consultar([FromQuery] string? nombre, [FromQuery] string? descripcion)
    {
        var resultado = await _examenService.ConsultarExamen(nombre, descripcion);
        return Ok(resultado);
    }
}
