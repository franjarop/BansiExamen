using Bansi.Examen.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bansi.Examen.Infrastructure.Persistence;

public class ExamenDbContext : DbContext
{
    public ExamenDbContext(DbContextOptions<ExamenDbContext> options) : base(options)
    {
    }

    public DbSet<RegistroExamen> Examenes => Set<RegistroExamen>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RegistroExamen>(entity =>
        {
            entity.ToTable("tblExamen");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("idExamen").ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasColumnName("Nombre").HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasColumnName("Descripcion").HasMaxLength(255);
        });
    }
}
