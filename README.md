# BansiExamen

Solución de práctica para el examen básico de selección de Bansi — mantenimiento de un catálogo (alta, baja, modificación y consulta) en arquitectura de capas, con Clean Architecture, inyección de dependencias y doble vía de acceso a datos (Stored Procedures o WebService), configurable en tiempo de ejecución.

## Arquitectura

```
Front (MVC)  →  DLL (AccesoDatos / ClsExamen)  →  ┬─ Stored Procedures (ADO.NET) ─┐
                                                     └─ WebService (Web API + EF Core) ─┴→ SQL Server
```

- **Front**: ASP.NET Core MVC. Solo conoce la DLL de acceso a datos.
- **AccesoDatos**: librería que decide, según configuración, si guarda por SP directos o llamando al WebService.
- **WebService**: ASP.NET Core Web API, accede a la base de datos únicamente con Entity Framework Core, con transaccionalidad.
- **Infrastructure**: implementaciones concretas (EF Core, ADO.NET, HttpClient) de la interfaz `IExamenGateway`.
- **Application / Domain**: reglas de negocio, casos de uso y contratos compartidos entre WebService y AccesoDatos.

Ver detalle completo de fases y decisiones técnicas en las notas de planeación del proyecto.

## Stack

- .NET 8 / C#
- ASP.NET Core Web API + MVC
- Entity Framework Core
- SQL Server LocalDB
- Inyección de dependencias nativa de .NET

## Estructura del repositorio

```
BansiExamen.sln
├─ database/        Scripts SQL (creación de BD, tabla y stored procedures)
└─ src/
   ├─ Bansi.Examen.Domain
   ├─ Bansi.Examen.Application
   ├─ Bansi.Examen.Infrastructure
   ├─ Bansi.Examen.WebService
   ├─ Bansi.Examen.AccesoDatos
   └─ Bansi.Examen.Front
```

## Cómo ejecutar

_Pendiente de documentar conforme se agreguen los proyectos._

## Estado

🚧 En desarrollo.
