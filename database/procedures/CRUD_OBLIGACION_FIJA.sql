CREATE OR ALTER PROCEDURE sp_insertar_obligacion
    @p_id_usuario int,
    @p_id_subcategoria int,
    @p_nombre_obligacion varchar(300),
    @p_descripcion_obligacion varchar(500),
    @p_monto decimal(12,2),
    @p_dia_vencimiento tinyint,
    @p_fecha_inicio date,
    @p_fecha_fin date,
    @p_creado_por int
AS
BEGIN
    INSERT INTO obligaciones_fijas(
        id_subcategoria,
        nombre_obligacion,
        monto_mensual,
        dia_vencimiento,
        estado_vigente,
        fecha_inicio,
        fecha_fin,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    )
    VALUES(
        @p_id_subcategoria,
        @p_nombre_obligacion,
        @p_monto,
        @p_dia_vencimiento,
        1,
        @p_fecha_inicio,
        @p_fecha_fin,
        @p_creado_por,
        @p_id_usuario,
        SYSDATETIME(),
        SYSDATETIME()
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_obligacion
    @p_id_obligacion int,
    @p_nombre_obligacion varchar(300),
    @p_descripcion_obligacion varchar(500),
    @p_monto decimal(12,2),
    @p_observaciones varchar(500),
    @p_modificado_por int,
    @p_dia_vencimiento tinyint,
    @p_fecha_fin date,
    @p_activo bit
AS 
BEGIN
    UPDATE obligaciones_fijas
    SET 
        nombre_obligacion = @p_nombre_obligacion,
        descripcion_obligacion = @p_descripcion_obligacion,
        monto_mensual = @p_monto,
        observaciones = @p_observaciones,
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_obligacion = @p_id_obligacion
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_obligacion
    @p_id_obligacion int,
    @p_modificado_por int
AS
BEGIN
    UPDATE obligaciones_fijas 
    SET estado_vigente =0
    WHERE id_obligacion = @p_id_obligacion
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_obligacion
    @p_id_obligacion int
AS
BEGIN 
    SELECT
       id_obligacion,
       id_subcategoria,
       nombre_obligacion,
       monto_mensual,
       dia_vencimiento,
       estado_vigente,
       fecha_inicio,
       fecha_fin,
       descripcion_obligacion,
       observaciones,
       creado_por,
       modificado_por,
       creado_en,
       modificado_en

    FROM obligaciones_fijas 
    WHERE id_obligacion = @p_id_obligacion
        
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_obligaciones_usuario
    @p_id_usuario INT,
    @p_activo bit
AS
BEGIN
    SELECT DISTINCT 
       o.id_obligacion,
       o.id_subcategoria,
       o.nombre_obligacion,
       o.monto_mensual,
       o.dia_vencimiento,
       o.estado_vigente,
       o.fecha_inicio,
       o.fecha_fin,
       o.descripcion_obligacion,
       o.observaciones,
       o.creado_por,
       o.modificado_por,
       o.creado_en,
       o.modificado_en
    FROM obligaciones_fijas o
    INNER JOIN subcategorias s ON 
    o.id_subcategoria = s.id_subcategoria
    INNER JOIN categorias c ON 
    s.id_categoria = c.id_categoria
    WHERE o.estado_vigente = @p_activo
    AND EXISTS (
          SELECT 1 
          FROM transacciones t 
          WHERE t.id_subcategoria = o.id_subcategoria 
            AND t.id_usuario = @p_id_usuario
      )
    
END;
GO