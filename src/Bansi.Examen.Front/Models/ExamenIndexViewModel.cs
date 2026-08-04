using Bansi.Examen.AccesoDatos;
using Bansi.Examen.Application.Dtos;

namespace Bansi.Examen.Front.Models;

public class ExamenIndexViewModel
{
    public ModoAcceso Modo { get; set; }
    public IEnumerable<ExamenDto> Examenes { get; set; } = Enumerable.Empty<ExamenDto>();
    public bool ConsultaExitosa { get; set; } = true;
    public string? MensajeError { get; set; }
}
