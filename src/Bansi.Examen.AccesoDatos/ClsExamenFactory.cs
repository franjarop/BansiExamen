using Bansi.Examen.Application.Interfaces;
using Bansi.Examen.Application.Services;
using Bansi.Examen.Infrastructure.Gateways;
using Bansi.Examen.Infrastructure.Repositories;

namespace Bansi.Examen.AccesoDatos;

public class ClsExamenFactory : IClsExamenFactory
{
    private readonly string _connectionString;
    private readonly IHttpClientFactory _httpClientFactory;

    public ClsExamenFactory(string connectionString, IHttpClientFactory httpClientFactory)
    {
        _connectionString = connectionString;
        _httpClientFactory = httpClientFactory;
    }

    public ClsExamen Crear(ModoAcceso modo)
    {
        IExamenGateway gateway = modo switch
        {
            ModoAcceso.StoredProcedure => new SqlExamenRepository(_connectionString),
            ModoAcceso.WebService => new HttpExamenGateway(_httpClientFactory.CreateClient(nameof(HttpExamenGateway))),
            _ => throw new ArgumentOutOfRangeException(nameof(modo), modo, "Modo de acceso no soportado")
        };

        var examenService = new ExamenService(gateway);
        return new ClsExamen(examenService);
    }
}
