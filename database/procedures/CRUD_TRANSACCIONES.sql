CREATE OR ALTER PROCEDURE sp_insertar_transaccion
    @p_id_usuario int,
    @p_id_presupuesto int,
    @p_id_detalle int,
    @p_anio_transaccion smallint,
    @p_mes_transaccion tinyint,
    @p_id_subcategoria int,
    @p_id_obligacion int,
    @p_tipo_transaccion varchar(100),
    @p_descripcion_movimiento varchar(300),
    @p_monto_transaccion decimal(12,2),
    @p_fecha_transaccion date,
    @p_metodo_pago varchar(100),
    @p_numero_factura int,
    @p_observaciones varchar(500),
    @p_creado_por int
AS
BEGIN
     INSERT INTO transacciones(
        id_usuario,
        id_detalle,
        anio_transaccion,
        mes_transaccion,
        id_subcategoria,
        tipo_transaccion,
        descripcion_movimiento,
        monto_transaccion,
        fecha_transaccion,
        metodo_pago,
        numero_factura,
        observaciones,
        fecha_y_hora_registro,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    )
    VALUES(
        @p_id_usuario,
        @p_id_detalle,
        @p_anio_transaccion,
        @p_mes_transaccion,
        @p_id_subcategoria,
        @p_tipo_transaccion,
        @p_descripcion_movimiento,
        @p_monto_transaccion,
        @p_fecha_transaccion,
        @p_metodo_pago,
        @p_numero_factura,
        @p_observaciones,
        SYSDATETIME(), 
        @p_creado_por, 
        @p_creado_por,  
        SYSDATETIME(),  
        SYSDATETIME()   
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_transaccion
    @p_id_transaccion int,
    @p_anio_transaccion smallint,
    @p_mes_transaccion tinyint,
    @p_descripcion_movimiento varchar(300),
    @p_monto_transaccion decimal(12,2),
    @p_fecha_transaccion date,
    @p_metodo_pago varchar(100),
    @p_numero_factura int,
    @p_observaciones varchar(500),
    @p_modificado_por int
AS 
BEGIN
    UPDATE transacciones
    SET 
        anio_transaccion = @p_anio_transaccion,
        mes_transaccion = @p_mes_transaccion,
        descripcion_movimiento = @p_descripcion_movimiento,
        monto_transaccion = @p_monto_transaccion,
        fecha_transaccion = @p_fecha_transaccion,
        metodo_pago = @p_metodo_pago,
        numero_factura = @p_numero_factura,
        observaciones = @p_observaciones,
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_transaccion = @p_id_transaccion
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_transaccion
    @p_id_transaccion int,
    @p_modificado_por int
AS
BEGIN
   DELETE FROM transacciones_obligaciones_fijas 
   WHERE id_transaccion = @p_id_transaccion

   DELETE FROM transacciones
   WHERE id_transaccion = @p_id_transaccion;
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_transaccion
    @p_id_transaccion int
AS
BEGIN
    SELECT 
        t.id_usuario,
        t.id_detalle,
        t.anio_transaccion,
        t.mes_transaccion,
        t.id_subcategoria,
        t.tipo_transaccion,
        t.descripcion_movimiento,
        t.monto_transaccion,
        t.fecha_transaccion,
        t.metodo_pago,
        t.numero_factura,
        t.observaciones,
        t.fecha_y_hora_registro,
        t.creado_por,
        t.modificado_por,
        t.creado_en,
        t.modificado_en
    FROM transacciones t
    WHERE t.id_transaccion = @p_id_transaccion;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_transacciones_presupuestos
    @p_id_presupuesto INT,
    @p_anio_transaccion smallint,
    @p_mes tinyint,
    @p_tipo_transaccion varchar(100)
AS
BEGIN
    SELECT DISTINCT 
        t.id_usuario,
        t.id_detalle,
        t.anio_transaccion,
        t.mes_transaccion,
        t.id_subcategoria,
        t.tipo_transaccion,
        t.descripcion_movimiento,
        t.monto_transaccion,
        t.fecha_transaccion,
        t.metodo_pago,
        t.numero_factura,
        t.observaciones,
        t.fecha_y_hora_registro,
        t.creado_por,
        t.modificado_por,
        t.creado_en,
        t.modificado_en
    FROM transacciones t
    INNER JOIN prespuesto_detalles pd ON
    t.id_detalle = pd.id_detalle
    INNER JOIN subcategorias s ON
    t.id_subcategoria = s.id_subcategoria
    INNER JOIN categorias c ON
    s.id_categoria = c.id_categoria
    WHERE pd.id_presupuesto = @p_id_presupuesto
END;
GO