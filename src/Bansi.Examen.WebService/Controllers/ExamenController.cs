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

    /// <summary>
    /// Agrega un examen mediante Entity Framework a la base de datos.
    /// </summary>
    /// <param name="examen">Nombre y descripción del examen a insertar.</param>
    /// <returns>Éxito/fracaso de la operación junto con una descripción.</returns>
    [HttpPost]
    public async Task<ActionResult<ResultadoOperacion>> Agregar(ExamenDto examen)
    {
        var resultado = await _examenService.AgregarExamen(examen);
        return Ok(resultado);
    }

    /// <summary>
    /// Actualiza un examen mediante Entity Framework a la base de datos.
    /// </summary>
    /// <param name="examen">Id del examen a actualizar, junto con su nuevo nombre y descripción.</param>
    /// <returns>Éxito/fracaso de la operación junto con una descripción.</returns>
    [HttpPut]
    public async Task<ActionResult<ResultadoOperacion>> Actualizar(ExamenDto examen)
    {
        var resultado = await _examenService.ActualizarExamen(examen);
        return Ok(resultado);
    }

    /// <summary>
    /// Elimina un examen mediante Entity Framework a la base de datos.
    /// </summary>
    /// <param name="id">Id del examen a eliminar.</param>
    /// <returns>Éxito/fracaso de la operación junto con una descripción.</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ResultadoOperacion>> Eliminar(int id)
    {
        var resultado = await _examenService.EliminarExamen(id);
        return Ok(resultado);
    }

    /// <summary>
    /// Devuelve una consulta mediante Entity Framework a la base de datos.
    /// </summary>
    /// <param name="nombre">Filtro opcional por nombre (búsqueda parcial).</param>
    /// <param name="descripcion">Filtro opcional por descripción (búsqueda parcial).</param>
    /// <returns>Colección de exámenes que cumplen los criterios de búsqueda.</returns>
    [HttpGet]
    public async Task<ActionResult<ResultadoConsulta>> Consultar([FromQuery] string? nombre, [FromQuery] string? descripcion)
    {
        var resultado = await _examenService.ConsultarExamen(nombre, descripcion);
        return Ok(resultado);
    }
}
