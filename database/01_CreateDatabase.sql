-- Crea la base de datos BdiExamen si no existe.
IF DB_ID(N'BdiExamen') IS NULL
BEGIN
    CREATE DATABASE BdiExamen;
END
GO
