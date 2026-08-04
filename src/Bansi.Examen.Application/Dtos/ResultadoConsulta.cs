namespace Bansi.Examen.Application.Dtos;

public class ResultadoConsulta
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public IEnumerable<ExamenDto> Datos { get; set; } = Enumerable.Empty<ExamenDto>();

    public static ResultadoConsulta Ok(IEnumerable<ExamenDto> datos) => new() { Exito = true, Mensaje = string.Empty, Datos = datos };

    public static ResultadoConsulta Error(string mensaje) => new() { Exito = false, Mensaje = mensaje };
}
