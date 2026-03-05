CREATE OR ALTER PROCEDURE sp_insertar_presupuesto_detalle
    @p_id_presupuesto int,
    @p_id_subcategoria int,
    @p_monto_mensual decimal (12,2),
    @p_observaciones varchar(500),
    @p_creado_por int
AS
BEGIN
    INSERT INTO prespuesto_detalles(
        id_presupuesto,
        id_subcategoria,
        monto_mensual,
        observaciones,
        creado_por,
        creado_en,
        modificado_en,
        modificado_por
    )
    VALUES(
        @p_id_presupuesto,
        @p_id_subcategoria,
        @p_monto_mensual,
        @p_observaciones,
        @p_creado_por,
        SYSDATETIME(),
        SYSDATETIME(),
        @p_creado_por
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_presupuesto_detalle
    @p_id_detalle int,
    @p_monto_mensual decimal(12,2),
    @p_observaciones varchar(500),
    @p_modificado_por int
AS 
BEGIN
    UPDATE prespuesto_detalles
    SET 
        monto_mensual = @p_monto_mensual,
        observaciones = @p_observaciones,
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_detalle= @p_id_detalle;
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_presupuesto_detalle
    @p_id_detalle int,
    @p_modificado_por int
AS
BEGIN
DELETE FROM prespuesto_detalles 
WHERE id_detalle = @p_id_detalle;
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_presupuesto_detalle
    @p_id_detalle int
AS
BEGIN 
    SELECT
       pd.id_detalle,
       pd.id_presupuesto,
       pd.id_subcategoria,
       pd.monto_mensual,
       pd.observaciones,
       pd.creado_por,
       pd.modificado_por,
       pd.creado_en,
       pd.modificado_en
    FROM prespuesto_detalles pd
    WHERE pd.id_detalle = @p_id_detalle
        
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_detalles_presupuesto
    @p_id_presupuesto INT
AS
BEGIN
    SELECT
       pd.id_detalle,
       pd.id_presupuesto,
       pd.id_subcategoria,
       pd.monto_mensual,
       pd.observaciones,
       pd.creado_por,
       pd.modificado_por,
       pd.creado_en,
       pd.modificado_en
    FROM prespuesto_detalles pd
    WHERE pd.id_presupuesto = @p_id_presupuesto;
END;
GO