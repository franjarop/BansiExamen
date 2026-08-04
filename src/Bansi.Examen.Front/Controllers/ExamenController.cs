using Bansi.Examen.AccesoDatos;
using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Front.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bansi.Examen.Front.Controllers;

public class ExamenController : Controller
{
    private readonly IClsExamenFactory _clsExamenFactory;

    public ExamenController(IClsExamenFactory clsExamenFactory)
    {
        _clsExamenFactory = clsExamenFactory;
    }

    public async Task<IActionResult> Index(ModoAcceso modo = ModoAcceso.StoredProcedure)
    {
        var cls = _clsExamenFactory.Crear(modo);
        var resultado = await cls.ConsultarExamen(null, null);

        var viewModel = new ExamenIndexViewModel
        {
            Modo = modo,
            Examenes = resultado.Datos,
            ConsultaExitosa = resultado.Exito,
            MensajeError = resultado.Exito ? null : resultado.Mensaje
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(ModoAcceso modo, string nombre, string descripcion)
    {
        var cls = _clsExamenFactory.Crear(modo);
        var resultado = await cls.AgregarExamen(new ExamenDto { Nombre = nombre, Descripcion = descripcion });

        TempData["Mensaje"] = resultado.Mensaje;
        TempData["Exito"] = resultado.Exito;
        return RedirectToAction(nameof(Index), new { modo });
    }

    [HttpPost]
    public async Task<IActionResult> Actualizar(ModoAcceso modo, int id, string nombre, string descripcion)
    {
        var cls = _clsExamenFactory.Crear(modo);
        var resultado = await cls.ActualizarExamen(new ExamenDto { Id = id, Nombre = nombre, Descripcion = descripcion });

        TempData["Mensaje"] = resultado.Mensaje;
        TempData["Exito"] = resultado.Exito;
        return RedirectToAction(nameof(Index), new { modo });
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(ModoAcceso modo, int id)
    {
        var cls = _clsExamenFactory.Crear(modo);
        var resultado = await cls.EliminarExamen(id);

        TempData["Mensaje"] = resultado.Mensaje;
        TempData["Exito"] = resultado.Exito;
        return RedirectToAction(nameof(Index), new { modo });
    }
}
