using System.Data;
using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Interfaces;
using Microsoft.Data.SqlClient;

namespace Bansi.Examen.Infrastructure.Repositories;

public class SqlExamenRepository : IExamenGateway
{
    private readonly string _connectionString;

    public SqlExamenRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task<ResultadoOperacion> AgregarAsync(ExamenDto examen) =>
        EjecutarSpEscritura("spAgregar", cmd =>
        {
            cmd.Parameters.AddWithValue("@Nombre", (object?)examen.Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)examen.Descripcion ?? DBNull.Value);
        });

    public Task<ResultadoOperacion> ActualizarAsync(ExamenDto examen) =>
        EjecutarSpEscritura("spActualizar", cmd =>
        {
            cmd.Parameters.AddWithValue("@Id", examen.Id);
            cmd.Parameters.AddWithValue("@Nombre", (object?)examen.Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)examen.Descripcion ?? DBNull.Value);
        });

    public Task<ResultadoOperacion> EliminarAsync(int id) =>
        EjecutarSpEscritura("spEliminar", cmd =>
        {
            cmd.Parameters.AddWithValue("@Id", id);
        });

    public async Task<ResultadoConsulta> ConsultarAsync(string? nombre, string? descripcion)
    {
        // Fallas de conexión/ejecución inesperadas se propagan (no se controlan aquí);
        // las atrapa el manejador de excepciones de la capa que consuma este repositorio.
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("spConsultar", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@Nombre", (object?)nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", (object?)descripcion ?? DBNull.Value);

        var resultados = new List<ExamenDto>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            resultados.Add(new ExamenDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? null : reader.GetString(reader.GetOrdinal("Nombre")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? null : reader.GetString(reader.GetOrdinal("Descripcion"))
            });
        }

        return ResultadoConsulta.Ok(resultados);
    }

    private async Task<ResultadoOperacion> EjecutarSpEscritura(string nombreSp, Action<SqlCommand> agregarParametros)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(nombreSp, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        agregarParametros(command);

        command.Parameters.Add(new SqlParameter("@CodigoRetorno", SqlDbType.Int) { Direction = ParameterDirection.Output });
        command.Parameters.Add(new SqlParameter("@DescripcionRetorno", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });

        await command.ExecuteNonQueryAsync();

        var codigoRetorno = (int)command.Parameters["@CodigoRetorno"].Value;
        var descripcionRetorno = command.Parameters["@DescripcionRetorno"].Value?.ToString() ?? string.Empty;

        return codigoRetorno == 0
            ? ResultadoOperacion.Ok(descripcionRetorno)
            : ResultadoOperacion.Error(descripcionRetorno);
    }
}
