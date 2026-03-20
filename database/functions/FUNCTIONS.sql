CREATE OR ALTER FUNCTION fn_calcular_monto_ejecutado
(@p_id_subcategoria int,
@p_anio smallint,
@p_mes tinyint)
RETURNS decimal(12,2)
as 
begin
	declare @resultado decimal(12,2);

	select @resultado = isnull(sum(t.monto_transaccion),0)
	from transacciones t
	where t.id_subcategoria = @p_id_subcategoria
		and t.anio_transaccion = @p_anio
		and t.mes_transaccion = @p_mes

	return @resultado;
end
go

CREATE OR ALTER FUNCTION fn_calcular_porcentaje_ejecutado
(@p_id_subcategoria int,
@p_id_presupuesto int,
@p_anio smallint,
@p_mes tinyint)
returns decimal(5,2)
as 
begin
	declare @porcentaje decimal(5,2)
	
	declare @monto_ejecutado decimal(12,2);
    declare @monto_presupuestado decimal(12,2);

	set  @monto_ejecutado = dbo.fn_calcular_monto_ejecutado(@p_id_subcategoria, @p_anio,@p_mes);

    select @monto_presupuestado = pd.monto_mensual
    from prespuesto_detalles pd
    where pd.id_presupuesto = @p_id_presupuesto
    and pd.id_subcategoria =@p_id_subcategoria;

    if @monto_presupuestado is null or @monto_presupuestado = 0
        set @porcentaje = 0;
    else
        set @porcentaje = (@monto_ejecutado / @monto_presupuestado) * 100;
	return @porcentaje;
end
go

CREATE OR ALTER FUNCTION fn_obtener_balance_subcategoria
(@p_id_presupuesto int,
@p_id_subcategoria int,
@p_anio smallint,
@p_mes tinyint)
returns decimal(12,2)
as
begin 
	declare @balance decimal(12,2)
	declare @ejecutado decimal(12,2)
	declare @presupuestado decimal(12,2)

	set @ejecutado =dbo.fn_calcular_monto_ejecutado(@p_id_subcategoria,@p_anio,@p_mes);

	select @presupuestado = pd.monto_mensual
	from prespuesto_detalles pd
	where pd.id_presupuesto = @p_id_presupuesto
	and pd.id_subcategoria = @p_id_subcategoria;

	set @balance = isnull(@presupuestado,0) - @ejecutado;

	return @balance
end
go

CREATE OR ALTER FUNCTION fn_obtener_total_categoria_mes
(@p_id_categoria int,
@p_id_presupuesto int,
@p_anio smallint,
@p_mes tinyint)
returns decimal(12,2)
as
begin
	declare @total decimal(12,2)
	
	select @total = isnull(sum(pd.monto_mensual), 0)
    from prespuesto_detalles pd
    inner join subcategorias s on pd.id_subcategoria = s.id_subcategoria
    where pd.id_presupuesto = @p_id_presupuesto
    and s.id_categoria   = @p_id_categoria;


	return @total
end
go

CREATE OR ALTER FUNCTION fn_obtener_total_ejecutado_categoria_mes
(@p_id_categoria int,
@p_anio smallint,
@p_mes tinyint)
returns decimal(12,2)
as
begin
	declare @total decimal(12,2)
	select @total = isnull(sum(t.monto_transaccion), 0)
    from transacciones t
    inner join subcategorias s on t.id_subcategoria = s.id_subcategoria
    where s.id_categoria   = @p_id_categoria
    and t.anio_transaccion = @p_anio
    and t.mes_transaccion  = @p_mes;
	return @total
end
go

CREATE OR ALTER FUNCTION fn_dias_hasta_vencimiento
(@p_id_obligacion int)
returns int
as
begin
	declare @dias int;
	declare @dia_vencimiento tinyint;

	select @dia_vencimiento = obf.dia_vencimiento
	from obligaciones_fijas obf
	where obf.id_obligacion = @p_id_obligacion;

	set @dias = DATEDIFF(
				DAY,
				CAST(GETDATE() as date),
				DATEFROMPARTS(YEAR(GETDATE()),MONTH(GETDATE()),@dia_vencimiento)
				);

	return @dias
end
go

CREATE OR ALTER FUNCTION fn_validar_vigencia_presupuesto
(@p_anio smallint,
@p_mes tinyint,
@p_id_presupuesto int)
returns bit
as 
begin
	declare @resultado bit;
	declare @anio_inicio smallint;
	declare @mes_inicio tinyint;
	declare @anio_fin smallint;
	declare @mes_fin tinyint;

	select 
		@anio_inicio = p.anio_inicio,
		@mes_inicio = p.mes_inicio,
		@anio_fin = p.anio_fin,
		@mes_fin = p.mes_fin
	from presupuestos p 
	where p.id_presupuesto = @p_id_presupuesto;

	if @anio_inicio is null
	begin
		set @resultado = 0;
		return @resultado;
	end

	if (@p_anio > @anio_inicio or (@p_anio = @anio_inicio and @p_mes >= @mes_inicio))
    and (@p_anio < @anio_fin   or (@p_anio = @anio_fin   and @p_mes <= @mes_fin))
        set @resultado = 1;
    else
        set @resultado = 0;

    return @resultado;
end
go

CREATE OR ALTER FUNCTION fn_obtener_categoria_por_subcategoria
(@p_id_subcategoria int)
returns int
as
begin
	declare @id_categoria int
	select @id_categoria = s.id_categoria
    from subcategorias s
    where s.id_subcategoria = @p_id_subcategoria;
	return @id_categoria
end
go

CREATE OR ALTER FUNCTION fn_calcular_proyeccion_gasto_mensual
(@p_id_subcategoria int,
@p_anio smallint,
@p_mes tinyint)
returns decimal(12,2) 
as
begin
	declare @proyeccion decimal(12,2);
	declare @gasto_actual decimal(12,2);
	declare @dias_transcurridos int;
	declare @dias_totales int;

	set @gasto_actual = dbo.fn_calcular_monto_ejecutado(@p_id_subcategoria,@p_anio,@p_mes)

	set @dias_transcurridos = DAY(GETDATE());

	set @dias_totales = DAY(EOMONTH(DATEFROMPARTS(@p_anio,@p_mes,1)));

	if @dias_transcurridos = 0
	begin
		set @proyeccion = 0;
		return @proyeccion;
	end

	set @proyeccion = (@gasto_actual / @dias_transcurridos) * @dias_totales;

	return @proyeccion;
end
go

CREATE OR ALTER FUNCTION fn_obtener_promedio_gasto_subcategoria
(@p_id_usuario int,
@p_id_subcategoria int,
@p_cantidad_meses tinyint)
returns decimal(12,2)
as
begin
	declare @promedio decimal(12,2);

	select @promedio = isnull(sum(t.monto_transaccion)/@p_cantidad_meses,0)
	from transacciones t
	where t.id_usuario = @p_id_usuario
	and t.id_subcategoria = @p_id_subcategoria
	and DATEFROMPARTS(t.anio_transaccion, t.mes_transaccion,1)>=
		DATEADD(MONTH, -@p_cantidad_meses, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1));

    return @promedio;
end
GO