CREATE OR ALTER PROCEDURE sp_crear_presupuesto_completo
@p_id_usuario int,
@p_nombre_presupuesto varchar(300),
@p_descripcion_presupuesto varchar(500),
@p_anio_inicio smallint,
@p_mes_inicio tinyint,
@p_anio_fin smallint,
@p_mes_fin tinyint,
@p_lista_subcategorias_json nvarchar(MAX), 
@p_creado_por int
AS
BEGIN

    DECLARE @total_items int;
    DECLARE @index int=0;
    DECLARE @id_subcategoria int;
    DECLARE @monto_mensual decimal (12,2);
    DECLARE @id_presupuesto int;
	
	EXEC sp_insertar_presupuesto
		@p_id_usuario = @p_id_usuario,
        @p_nombre_presupuesto = @p_nombre_presupuesto,
        @p_anio_inicio = @p_anio_inicio,
        @p_mes_inicio = @p_mes_inicio,
        @p_mes_fin = @p_mes_fin,
        @p_anio_fin = @p_anio_fin,
        @p_descripcion_presupuesto = @p_descripcion_presupuesto,
        @p_id_presupuesto = @id_presupuesto OUTPUT;

    SELECT @total_items = COUNT(*)
    FROM openjson(@p_lista_subcategorias_json);

    WHILE @index < @total_items
    BEGIN
        SELECT 
            @id_subcategoria = JSON_VALUE(value,'$.id_subcategoria'),
            @monto_mensual = JSON_VALUE(value,'$.monto_mensual')
        FROM openjson(@p_lista_subcategorias_json)
        WHERE [key]  = @index;

        EXEC sp_insertar_presupuesto_detalle
            @p_id_presupuesto = @id_presupuesto,
            @p_id_subcategoria = @id_subcategoria,
            @p_monto_mensual = @monto_mensual,
            @p_creado_por = @p_creado_por;

        SET @index = @index +1;
    END;

    
END

