# BansiExamen

Proyecto de práctica para el examen básico de selección de Bansi: un catálogo simple (alta, baja, modificación y consulta), pero
resuelto con  Clean Architecture, inyección de dependencias, y la posibilidad de guardar los datos por Stored Procedures o
por un WebService, cambiando entre uno y otro desde la pantalla.

## Arquitectura
Clean arquitecture en capas con doble via de acceso a los datos:
-web services
-Store procedure

El proyecto esta conformado por 6 capas:
-Dominio
-Aplication
-Infraestructure 
-Web service
-DLL (Acceso a los datos)
-Front 

## Stack

- .NET 8 / C#
- ASP.NET Core Web API + MVC
- Entity Framework Core
- SQL Server
- Inyección de dependencias nativa de .NET

## Estructura del repositorio

```
BansiExamen.sln
├─ database/                       Scripts SQL (creación de BD, tabla y stored procedures)
└─ src/
   ├─ Bansi.Examen.Domain          Entidad RegistroExamen
   ├─ Bansi.Examen.Application     DTOs, IExamenGateway, ExamenService, validaciones
   ├─ Bansi.Examen.Infrastructure  Repositories/ (EF Core, ADO.NET) y Gateways/ (HTTP)
   ├─ Bansi.Examen.WebService      Web API (ExamenController + middleware de excepciones)
   ├─ Bansi.Examen.AccesoDatos     ClsExamen + ClsExamenFactory (la "apiexamen.dll")
   └─ Bansi.Examen.Front           MVC (formularios, listado, selector de modo)
```

## Cómo ejecutar

### Base de datos

Los scripts están en el folder `database/` y hay que correrlos en ese orden, porque cada uno depende del anterior:

1. `01_CreateDatabase.sql` — crea la base `BdiExamen`
2. `02_CreateTable.sql` — crea la tabla `tblExamen`
3. `03_StoredProcedures.sql` — crea `spAgregar`, `spActualizar`, `spEliminar` y `spConsultar`

### WebService y Front

Se necesitan los dos corriendo al mismo tiempo, cada uno en su propia terminal:

```
cd src/Bansi.Examen.WebService && dotnet run
```
```
cd src/Bansi.Examen.Front && dotnet run
```

El Front busca el WebService en `http://localhost:5194/` (configurable en `src/Bansi.Examen.Front/appsettings.json`, clave `WebServiceBaseUrl`).

En Visual Studio: clic derecho en la solución → **Set Startup Projects** → **Multiple startup projects** → poner ambos en **Start**.

Entra al Front y da clic en **Examen** en el menú. Desde ahí puedes:
- Elegir el modo de acceso (Stored Procedures o WebService) con el selector de arriba.
- Agregar, actualizar y eliminar registros.
- Consultar el listado, con estilo de filas alternadas.

Toda operación muestra un mensaje de éxito o error en pantalla, sin importar el modo elegido.

El WebService expone Swagger en modo desarrollo (`/swagger`), con la documentación de cada endpoint.

