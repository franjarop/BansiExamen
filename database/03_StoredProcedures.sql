USE BdiExamen;
GO

CREATE OR ALTER PROCEDURE dbo.spAgregar
    @Nombre             VARCHAR(255),
    @Descripcion        VARCHAR(255),
    @CodigoRetorno      INT OUTPUT,
    @DescripcionRetorno VARCHAR(255) OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.tblExamen (Nombre, Descripcion)
        VALUES (@Nombre, @Descripcion);

        COMMIT TRANSACTION;

        SET @CodigoRetorno = 0;
        SET @DescripcionRetorno = 'Registro insertado satisfactoriamente';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        SET @CodigoRetorno = ERROR_NUMBER();
        SET @DescripcionRetorno = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.spActualizar
    @Id                 INT,
    @Nombre             VARCHAR(255),
    @Descripcion        VARCHAR(255),
    @CodigoRetorno      INT OUTPUT,
    @DescripcionRetorno VARCHAR(255) OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.tblExamen WHERE idExamen = @Id)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @CodigoRetorno = 1;
            SET @DescripcionRetorno = 'No existe un registro con el Id especificado';
            RETURN;
        END

        UPDATE dbo.tblExamen
        SET Nombre = @Nombre,
            Descripcion = @Descripcion
        WHERE idExamen = @Id;

        COMMIT TRANSACTION;

        SET @CodigoRetorno = 0;
        SET @DescripcionRetorno = 'Registro actualizado satisfactoriamente';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        SET @CodigoRetorno = ERROR_NUMBER();
        SET @DescripcionRetorno = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.spEliminar
    @Id                 INT,
    @CodigoRetorno      INT OUTPUT,
    @DescripcionRetorno VARCHAR(255) OUTPUT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM dbo.tblExamen WHERE idExamen = @Id)
        BEGIN
            ROLLBACK TRANSACTION;
            SET @CodigoRetorno = 1;
            SET @DescripcionRetorno = 'No existe un registro con el Id especificado';
            RETURN;
        END

        DELETE FROM dbo.tblExamen WHERE idExamen = @Id;

        COMMIT TRANSACTION;

        SET @CodigoRetorno = 0;
        SET @DescripcionRetorno = 'Registro eliminado satisfactoriamente';
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        SET @CodigoRetorno = ERROR_NUMBER();
        SET @DescripcionRetorno = ERROR_MESSAGE();
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.spConsultar
    @Nombre      VARCHAR(255) = NULL,
    @Descripcion VARCHAR(255) = NULL
AS
BEGIN

    SELECT
        idExamen AS Id,
        Nombre,
        Descripcion
    FROM dbo.tblExamen
    WHERE (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
      AND (@Descripcion IS NULL OR Descripcion LIKE '%' + @Descripcion + '%')
    ORDER BY idExamen;
END
GO
