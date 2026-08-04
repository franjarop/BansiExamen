namespace Bansi.Examen.Application.Validation;

internal static class ExamenValidator
{
    private const int LongitudMaxima = 255;

    public static string? ValidarDatos(string? nombre, string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return "El nombre es requerido.";

        if (nombre.Length > LongitudMaxima)
            return $"El nombre no puede superar los {LongitudMaxima} caracteres.";

        if (string.IsNullOrWhiteSpace(descripcion))
            return "La descripción es requerida.";

        if (descripcion.Length > LongitudMaxima)
            return $"La descripción no puede superar los {LongitudMaxima} caracteres.";

        return null;
    }

    public static string? ValidarId(int id) => id > 0 ? null : "El Id debe ser mayor a cero.";
}
