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
    SET NOCOUNT ON;

    DECLARE @total_items int;
    DECLARE @index int = 0;
    DECLARE @id_subcategoria int;
    DECLARE @monto_mensual decimal(12,2);
    DECLARE @id_presupuesto int;

    BEGIN TRANSACTION
    BEGIN TRY

        EXEC sp_insertar_presupuesto
            @p_id_usuario= @p_id_usuario,
            @p_nombre_presupuesto= @p_nombre_presupuesto,
            @p_anio_inicio= @p_anio_inicio,
            @p_mes_inicio= @p_mes_inicio,
            @p_mes_fin= @p_mes_fin,
            @p_anio_fin= @p_anio_fin,
            @p_descripcion_presupuesto = @p_descripcion_presupuesto,
            @p_id_presupuesto= @id_presupuesto OUTPUT;

        SELECT @total_items = COUNT(*)
        FROM OPENJSON(@p_lista_subcategorias_json);

        WHILE @index < @total_items
        BEGIN
            SELECT 
                @id_subcategoria = JSON_VALUE(value, '$.id_subcategoria'),
                @monto_mensual   = JSON_VALUE(value, '$.monto_mensual')
            FROM OPENJSON(@p_lista_subcategorias_json)
            WHERE [key] = @index;

            EXEC sp_insertar_presupuesto_detalle
                @p_id_presupuesto  = @id_presupuesto,
                @p_id_subcategoria = @id_subcategoria,
                @p_monto_mensual   = @monto_mensual,
                @p_creado_por      = @p_creado_por;

            SET @index = @index + 1;
        END

        COMMIT TRANSACTION

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_registrar_transaccion_completa
    @p_id_usuario             int,
    @p_id_presupuesto         int,
    @p_anio                   smallint,
    @p_mes                    tinyint,
    @p_id_subcategoria        int,
    @p_tipo_transaccion       varchar(100),
    @p_descripcion_movimiento varchar(300),
    @p_monto_transaccion      decimal(12,2),
    @p_fecha_transaccion      date,
    @p_metodo_pago            varchar(100),
    @p_creado_por             int
AS 
BEGIN
    declare @anio_inicio smallint;
    declare @mes_inicio  tinyint;
    declare @anio_fin    smallint;
    declare @mes_fin     tinyint;
    declare @id_detalle  int;

    select
        @anio_inicio = p.anio_inicio,
        @mes_inicio  = p.mes_inicio,
        @anio_fin    = p.anio_fin,
        @mes_fin     = p.mes_fin
    from presupuestos p 
    where p.id_presupuesto   = @p_id_presupuesto
      AND p.estado_presupuesto = 'activo';  

    if @anio_inicio is null
    begin
        raiserror('El presupuesto no existe o no esta activo', 16, 1);
        return;
    end

    if @p_anio <= 0 or (@p_mes <= 0 or @p_mes > 12)
    begin 
        raiserror('Fecha ingresada no valida', 16, 1);
        return;
    end

    if @p_anio < @anio_inicio or @p_anio > @anio_fin
    begin 
        raiserror('La transaccion no esta dentro del rango valido para el presupuesto', 16, 1);
        return;
    end

    select @id_detalle = pd.id_detalle
    from prespuesto_detalles pd
    where pd.id_presupuesto  = @p_id_presupuesto
      and pd.id_subcategoria = @p_id_subcategoria;

    if @id_detalle is null
    begin
        raiserror('La subcategoria no esta en los detalles del presupuesto', 16, 1);
        return;
    end

    EXEC sp_insertar_transaccion
        @p_id_usuario             = @p_id_usuario,
        @p_id_presupuesto         = @p_id_presupuesto,
        @p_id_detalle             = @id_detalle,
        @p_anio_transaccion       = @p_anio,
        @p_mes_transaccion        = @p_mes,
        @p_id_subcategoria        = @p_id_subcategoria,
        @p_id_obligacion          = NULL,
        @p_tipo_transaccion       = @p_tipo_transaccion,
        @p_descripcion_movimiento = @p_descripcion_movimiento,
        @p_monto_transaccion      = @p_monto_transaccion,
        @p_fecha_transaccion      = @p_fecha_transaccion,
        @p_metodo_pago            = @p_metodo_pago,
        @p_numero_factura         = NULL,
        @p_observaciones          = NULL,
        @p_creado_por             = @p_creado_por;
END
GO

CREATE OR ALTER PROCEDURE sp_procesar_obligaciones_mes
    @p_id_usuario     int,
    @p_anio           smallint,
    @p_mes            tinyint,
    @p_id_presupuesto int
AS
BEGIN 
    SELECT
        obf.id_obligacion,
        obf.nombre_obligacion,
        obf.monto_mensual,
        obf.dia_vencimiento,
        DATEFROMPARTS(@p_anio, @p_mes, obf.dia_vencimiento) AS fecha_vencimiento_mes,
        DATEDIFF(DAY, CAST(SYSDATETIME() AS date), DATEFROMPARTS(@p_anio, @p_mes, obf.dia_vencimiento)) AS dias_hasta_vencer,  -- ? coma agregada
        CASE
            WHEN EXISTS(
                SELECT 1
                FROM transacciones t
                INNER JOIN transacciones_obligaciones_fijas tof
                    ON t.id_transaccion = tof.id_transaccion
                WHERE tof.id_obligacion = obf.id_obligacion
                  AND t.anio_transaccion = @p_anio
                  AND t.mes_transaccion  = @p_mes
            ) THEN 1 
            ELSE 0
        END AS ya_pagada,                     
        s.nombre_subcategoria,
        c.nombre_categoria
    FROM obligaciones_fijas obf               
    INNER JOIN subcategorias s ON obf.id_subcategoria = s.id_subcategoria
    INNER JOIN categorias    c ON s.id_categoria      = c.id_categoria
    WHERE obf.creado_por    = @p_id_usuario     
      AND obf.estado_vigente = 1               
      AND obf.fecha_inicio  <= DATEFROMPARTS(@p_anio, @p_mes, 28)  
      AND (
            obf.fecha_fin IS NULL              
            OR obf.fecha_fin >= DATEFROMPARTS(@p_anio, @p_mes, 1)  
          )
    ORDER BY obf.dia_vencimiento ASC;          
END
GO

CREATE OR ALTER PROCEDURE sp_calcular_balance_mensual
@p_id_usuario int,
@p_id_presupuesto int,
@p_anio smallint,
@p_mes tinyint,
@p_total_ingresos decimal(12,2) output,
@p_total_gastos decimal(12,2) output,
@p_total_ahorros decimal(12,2) output,
@p_balance_final decimal(12,2) output
as
begin
   if @p_mes<=0 or @p_mes>12
   begin
   raiserror('El mes ingresado no es valido',16,1);
   return;
   end

   select @p_total_ingresos = ISNULL(SUM(t.monto_transaccion),0)
   from transacciones t
   where t.id_usuario = @p_id_usuario
   and t.id_detalle in(
   select pd.id_detalle
   from prespuesto_detalles pd
   where pd.id_presupuesto = @p_id_presupuesto
   )
   and t.anio_transaccion = @p_anio
   and t.mes_transaccion = @p_mes
   and t.tipo_transaccion = 'ingreso';
   
   select @p_total_gastos = ISNULL(SUM(t.monto_transaccion),0)
   from transacciones t
   where t.id_usuario = @p_id_usuario
   and t.id_detalle in(
   select pd.id_detalle
   from prespuesto_detalles pd
   where pd.id_presupuesto = @p_id_presupuesto
   )
   and t.anio_transaccion = @p_anio
   and t.mes_transaccion = @p_mes
   and t.tipo_transaccion = 'gasto';

   select @p_total_ahorros = ISNULL(SUM(t.monto_transaccion),0)
   from transacciones t
   where t.id_usuario = @p_id_usuario
   and t.id_detalle in(
   select pd.id_detalle
   from prespuesto_detalles pd
   where pd.id_presupuesto =@p_id_presupuesto
   )
   and t.anio_transaccion = @p_anio
   and t.mes_transaccion = @p_mes
   and t.tipo_transaccion='ahorro'

   set @p_balance_final = @p_total_ingresos - @p_total_gastos - @p_total_ahorros;
end;
go

CREATE OR ALTER PROCEDURE sp_calcular_monto_ejecutado_mes
@p_id_subcategoria int,
@p_id_presupuesto int,
@p_anio smallint,
@p_mes tinyint,
@p_monto_ejecutado decimal(12,2) output
as
begin

select @p_monto_ejecutado = ISNULL(SUM(t.monto_transaccion),0)
from transacciones t
inner join prespuesto_detalles pd on
t.id_detalle = pd.id_detalle
where pd.id_presupuesto = @p_id_presupuesto
and t.id_subcategoria = @p_id_subcategoria
and t.anio_transaccion = @p_anio
and t.mes_transaccion = @p_mes
end;
go

CREATE OR ALTER PROCEDURE sp_calcular_porcentaje_ejecucion_mes
@p_id_subcategoria int,
@p_id_presupuesto int,
@p_anio int,
@p_mes int,
@p_porcentaje decimal(5,2) output
as 
begin
    declare @monto_ejecutado decimal(12,2);
    declare @monto_presupuestado decimal(12,2);

    exec sp_calcular_monto_ejecutado_mes
        @p_id_subcategoria = @p_id_subcategoria,
        @p_id_presupuesto  = @p_id_presupuesto,
        @p_anio= @p_anio,
        @p_mes= @p_mes,
        @p_monto_ejecutado = @monto_ejecutado OUTPUT;

    select @monto_presupuestado = pd.monto_mensual
    from prespuesto_detalles pd
    where pd.id_presupuesto = @p_id_presupuesto
    and pd.id_subcategoria =@p_id_subcategoria;

    if @monto_presupuestado is null or @monto_presupuestado = 0
        set @p_porcentaje = 0;
    else
        set @p_porcentaje = (@monto_ejecutado / @monto_presupuestado) * 100;

end; 
go

CREATE OR ALTER PROCEDURE sp_cerrar_presupuesto
    @p_id_presupuesto int,
    @p_modificado_por int,
    @p_total_ingresos decimal(12,2) output,
    @p_total_gastos decimal(12,2) output,
    @p_total_ahorros  decimal(12,2) output
as
begin
    declare @estado bit;
    declare @anio_fin smallint;
    declare @mes_fin  tinyint;

    select
        @estado = p.estado_presupuesto,
        @anio_fin = p.anio_fin,
        @mes_fin  = p.mes_fin
    from presupuestos p
    where p.id_presupuesto = @p_id_presupuesto;

    if @estado is null or @estado != 1
    begin
        raiserror('Presupuesto no encontrado o no activo', 16, 1);
        return;
    end

    if @anio_fin > year(getdate())
       or (@anio_fin = year(getdate()) and @mes_fin >= month(getdate()))
    begin
        raiserror('El presupuesto aun no ha llegado a su fecha de fin', 16, 1);
        return;
    end

    select @p_total_ingresos = isnull(sum(t.monto_transaccion), 0)
    from transacciones t
    inner join prespuesto_detalles pd on t.id_detalle = pd.id_detalle  
    where pd.id_presupuesto  = @p_id_presupuesto
      and t.tipo_transaccion = 'ingreso';                              

    select @p_total_gastos = isnull(sum(t.monto_transaccion), 0)
    from transacciones t
    inner join prespuesto_detalles pd on t.id_detalle = pd.id_detalle
    where pd.id_presupuesto  = @p_id_presupuesto
      and t.tipo_transaccion = 'gasto';

    select @p_total_ahorros = isnull(sum(t.monto_transaccion), 0)
    from transacciones t
    inner join prespuesto_detalles pd on t.id_detalle = pd.id_detalle
    where pd.id_presupuesto  = @p_id_presupuesto
      and t.tipo_transaccion = 'ahorro';

    update presupuestos
    set
        estado_presupuesto = 0,
        modificado_por= @p_modificado_por,
        modificado_en = getdate()
    where id_presupuesto = @p_id_presupuesto;

end
GO


CREATE OR ALTER PROCEDURE sp_obtener_resumen_categoria_mes
    @p_id_categoria int,
    @p_id_presupuesto int,
    @p_anio smallint,
    @p_mes tinyint,
    @p_monto_presupuestado decimal(12,2) output,
    @p_monto_ejecutado decimal(12,2) output,
    @p_porcentaje decimal(5,2)  output
as
begin
    select @p_monto_presupuestado = isnull(sum(pd.monto_mensual), 0)
    from prespuesto_detalles pd
    inner join subcategorias s on pd.id_subcategoria = s.id_subcategoria
    where pd.id_presupuesto = @p_id_presupuesto
      and s.id_categoria = @p_id_categoria;

    select @p_monto_ejecutado = isnull(sum(t.monto_transaccion), 0)
    from transacciones t
    inner join subcategorias s on t.id_subcategoria = s.id_subcategoria
    inner join prespuesto_detalles pd on t.id_detalle = pd.id_detalle
    where pd.id_presupuesto  = @p_id_presupuesto
      and s.id_categoria = @p_id_categoria
      and t.anio_transaccion = @p_anio
      and t.mes_transaccion  = @p_mes;

    if @p_monto_presupuestado = 0
        set @p_porcentaje = 0;
    else
        set @p_porcentaje = (@p_monto_ejecutado / @p_monto_presupuestado) * 100;

end
GO