USE BdiExamen;
GO

IF OBJECT_ID(N'dbo.tblExamen', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblExamen
    (
        idExamen    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre      VARCHAR(255) NULL,
        Descripcion VARCHAR(255) NULL
    );
END
GO
