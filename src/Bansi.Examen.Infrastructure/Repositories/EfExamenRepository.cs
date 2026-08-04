using Bansi.Examen.Application.Dtos;
using Bansi.Examen.Application.Interfaces;
using Bansi.Examen.Domain;
using Bansi.Examen.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bansi.Examen.Infrastructure.Repositories;

public class EfExamenRepository : IExamenGateway
{
    private readonly ExamenDbContext _context;

    public EfExamenRepository(ExamenDbContext context)
    {
        _context = context;
    }

    public async Task<ResultadoOperacion> AgregarAsync(ExamenDto examen)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var entidad = new RegistroExamen { Nombre = examen.Nombre, Descripcion = examen.Descripcion };
            _context.Examenes.Add(entidad);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ResultadoOperacion.Ok("Registro insertado satisfactoriamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ResultadoOperacion.Error(ex.Message);
        }
    }

    public async Task<ResultadoOperacion> ActualizarAsync(ExamenDto examen)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var entidad = await _context.Examenes.FirstOrDefaultAsync(e => e.Id == examen.Id);
            if (entidad is null)
            {
                await transaction.RollbackAsync();
                return ResultadoOperacion.Error("No existe un registro con el Id especificado");
            }

            entidad.Nombre = examen.Nombre;
            entidad.Descripcion = examen.Descripcion;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ResultadoOperacion.Ok("Registro actualizado satisfactoriamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ResultadoOperacion.Error(ex.Message);
        }
    }

    public async Task<ResultadoOperacion> EliminarAsync(int id)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var entidad = await _context.Examenes.FirstOrDefaultAsync(e => e.Id == id);
            if (entidad is null)
            {
                await transaction.RollbackAsync();
                return ResultadoOperacion.Error("No existe un registro con el Id especificado");
            }

            _context.Examenes.Remove(entidad);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ResultadoOperacion.Ok("Registro eliminado satisfactoriamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ResultadoOperacion.Error(ex.Message);
        }
    }

    public async Task<ResultadoConsulta> ConsultarAsync(string? nombre, string? descripcion)
    {
        try
        {
            var query = _context.Examenes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(e => e.Nombre != null && e.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(descripcion))
                query = query.Where(e => e.Descripcion != null && e.Descripcion.Contains(descripcion));

            var resultados = await query
                .OrderBy(e => e.Id)
                .Select(e => new ExamenDto { Id = e.Id, Nombre = e.Nombre, Descripcion = e.Descripcion })
                .ToListAsync();

            return ResultadoConsulta.Ok(resultados);
        }
        catch (Exception ex)
        {
            return ResultadoConsulta.Error(ex.Message);
        }
    }
}
