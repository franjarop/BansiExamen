# BansiExamen

Proyecto de práctica para el examen básico de selección de Bansi: un catálogo simple (alta, baja, modificación y consulta), pero resuelto con arquitectura en capas de verdad — Clean Architecture, inyección de dependencias, y la posibilidad de guardar los datos por Stored Procedures o por un WebService, cambiando entre uno y otro.

## Arquitectura

```
Front (MVC)  →  DLL (AccesoDatos / ClsExamen)  →  ┬─ Stored Procedures (ADO.NET) ─┐
                                                     └─ WebService (Web API + EF Core) ─┴→ SQL Server
```

El Front solo le habla a la DLL de acceso a datos, nunca directo a SQL ni al WebService. La DLL es la que decide, según lo que elijas en pantalla, si guarda por SP o por WebService. El WebService, aparte, solo usa Entity Framework Core (nada de SPs ahí, es requisito). Infrastructure tiene las tres formas reales de guardar datos (EF, ADO.NET, HTTP) escondidas detrás de una sola interfaz, y Application/Domain traen las reglas de negocio que comparten el WebService y la DLL para no duplicar validaciones.

## Stack

- .NET 8 / C#
- ASP.NET Core Web API + MVC
- Entity Framework Core
- SQL Server 
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

### Base de datos

Los scripts están en el folder `database/` y hay que correrlos en ese orden, porque cada uno depende del anterior:

1. `01_CreateDatabase.sql` — crea la base `BdiExamen`
2. `02_CreateTable.sql` — crea la tabla `tblExamen`
3. `03_StoredProcedures.sql` — crea `spAgregar`, `spActualizar`, `spEliminar` y `spConsultar`

### Resto de la solución

Todavía no — falta armar WebService, AccesoDatos y Front.

## Estado

🚧 En desarrollo.
