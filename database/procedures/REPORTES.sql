CREATE OR ALTER PROCEDURE sp_reporte_resumen_mensual
@p_id_usuario int,
@p_anio_inicio smallint,
@p_mes_inicio tinyint,
@p_anio_fin smallint,
@p_mes_fin tinyint
as
begin
	select	
		t.anio_transaccion as anio,
		t.mes_transaccion as mes,
		sum(case when t.tipo_transaccion = 'ingreso' then t.monto_transaccion else 0 end) as total_ingresos,
		sum(case when t.tipo_transaccion = 'gasto' then t.monto_transaccion else 0 end) as total_gastos,
		sum(case when t.tipo_transaccion = 'ahorro' then t.monto_transaccion else 0 end) as total_ahorros,
		sum(case when t.tipo_transaccion != 'ingreso' and t.tipo_transaccion!='gasto' and t.tipo_transaccion!='ahorro' then t.monto_transaccion else 0 end) as otros,
		sum(case when t.tipo_transaccion = 'ingreso' then t.monto_transaccion else 0 end)  - 
		sum(case when t.tipo_transaccion = 'gasto' then t.monto_transaccion else 0 end) - 
		sum(case when t.tipo_transaccion = 'ahorro' then t.monto_transaccion else 0 end)-
		sum(case when t.tipo_transaccion != 'ingreso' and t.tipo_transaccion!='gasto' and t.tipo_transaccion!='ahorro' then t.monto_transaccion else 0 end) 
		as balance_final

	from transacciones t
	where t.id_usuario = @p_id_usuario
	and (t.anio_transaccion  > @p_anio_inicio
	or (t.anio_transaccion = @p_anio_inicio and t.mes_transaccion>=@p_mes_inicio)
	)
	and(
		t.anio_transaccion <@p_anio_fin
		or (t.anio_transaccion= @p_anio_fin and t.mes_transaccion <= @p_mes_fin)
	)

	group by t.anio_transaccion, t.mes_transaccion
	order by t.anio_transaccion, t.mes_transaccion;
end
go

CREATE OR ALTER PROCEDURE sp_reporte_distribucion_gastos
	@p_id_usuario int,
	@p_anio smallint,
	@p_mes tinyint
as
begin
	select
		c.nombre_categoria,
		sum(t.monto_transaccion)                                              as total_gastado,
		count(t.id_transaccion)                                               as num_transacciones,
		sum(t.monto_transaccion) * 100.0 / 
			sum(sum(t.monto_transaccion)) over()                              as porcentaje
	from transacciones t
	inner join subcategorias s  
	on t.id_subcategoria  = s.id_subcategoria
	inner join categorias c     
	on s.id_categoria= c.id_categoria
	where t.id_usuario= @p_id_usuario
	  and t.tipo_transaccion= 'gasto'
	  and t.anio_transaccion= @p_anio
	  and t.mes_transaccion= @p_mes
	group by c.id_categoria, c.nombre_categoria
	order by total_gastado desc;
end
go

CREATE OR ALTER PROCEDURE sp_reporte_cumplimiento_presupuesto
	@p_id_usuario int,
	@p_id_presupuesto int,
	@p_anio smallint,
	@p_mes tinyint
as
begin
	select
		c.nombre_categoria,
		s.nombre_subcategoria,
		pd.monto_mensual as monto_presupuestado,
		isnull(sum(t.monto_transaccion), 0) as monto_ejecutado,
		pd.monto_mensual - isnull(sum(t.monto_transaccion), 0) as diferencia,
		case
			when pd.monto_mensual = 0 then 0
			else isnull(sum(t.monto_transaccion), 0) * 100.0 / pd.monto_mensual
		end as porcentaje_ejecucion
	from prespuesto_detalles pd
	inner join subcategorias s  
	on pd.id_subcategoria = s.id_subcategoria
	inner join categorias c     
	on s.id_categoria= c.id_categoria
	inner join presupuestos p   
	on pd.id_presupuesto= p.id_presupuesto  
	left join transacciones t   
	on t.id_detalle= pd.id_detalle
	and t.anio_transaccion= @p_anio
	and t.mes_transaccion= @p_mes
	where pd.id_presupuesto  = @p_id_presupuesto
	  and p.id_usuario= @p_id_usuario      
	group by c.id_categoria, c.nombre_categoria, s.id_subcategoria,
	         s.nombre_subcategoria, pd.monto_mensual
	order by c.nombre_categoria, s.nombre_subcategoria;
end
go

CREATE OR ALTER PROCEDURE sp_reporte_tendencia_gastos
	@p_id_usuario int,
	@p_anio_inicio smallint,
	@p_mes_inicio tinyint,
	@p_anio_fin smallint,
	@p_mes_fin tinyint
as
begin
	select
		t.anio_transaccion as anio,
		t.mes_transaccion as mes,
		c.nombre_categoria,
		sum(t.monto_transaccion) as total_gastado
	from transacciones t
	inner join subcategorias s  
	on t.id_subcategoria = s.id_subcategoria
	inner join categorias c     
	on s.id_categoria= c.id_categoria
	where t.id_usuario= @p_id_usuario
	  and t.tipo_transaccion = 'gasto'
	  and (
			t.anio_transaccion > @p_anio_inicio
			or (t.anio_transaccion = @p_anio_inicio and t.mes_transaccion >= @p_mes_inicio)
		  )
	  and (
			t.anio_transaccion < @p_anio_fin
			or (t.anio_transaccion = @p_anio_fin and t.mes_transaccion <= @p_mes_fin)
		  )
	group by t.anio_transaccion, t.mes_transaccion, c.id_categoria, c.nombre_categoria
	order by t.anio_transaccion, t.mes_transaccion, c.nombre_categoria;
end
go

CREATE OR ALTER PROCEDURE sp_reporte_obligaciones_fijas
	@p_id_usuario int,
	@p_anio smallint,
	@p_mes tinyint
as
begin
	select
		obf.nombre_obligacion,
		c.nombre_categoria,
		obf.monto_mensual,
		obf.dia_vencimiento,
		datefromparts(@p_anio, @p_mes, obf.dia_vencimiento) as fecha_vencimiento,
		datediff(day, cast(getdate() as date),
			datefromparts(@p_anio, @p_mes, obf.dia_vencimiento))as dias_hasta_vencer,
		case
			when exists(
				select 1 from transacciones t
				inner join transacciones_obligaciones_fijas tof
					on t.id_transaccion = tof.id_transaccion
				where tof.id_obligacion    = obf.id_obligacion
				  and t.anio_transaccion   = @p_anio
				  and t.mes_transaccion    = @p_mes
			) then 'Pagada'
			when datediff(day, cast(getdate() as date),
				datefromparts(@p_anio, @p_mes, obf.dia_vencimiento)) < 0
				then 'Vencida'
			when datediff(day, cast(getdate() as date),
				datefromparts(@p_anio, @p_mes, obf.dia_vencimiento)) <= 3
				then 'Por vencer'
			else 'Pendiente'
		end as estado_pago
	from obligaciones_fijas obf
	inner join subcategorias s  
	on obf.id_subcategoria = s.id_subcategoria
	inner join categorias c     
	on s.id_categoria = c.id_categoria
	where obf.creado_por= @p_id_usuario
	  and obf.estado_vigente = 1
	  and obf.fecha_inicio  <= datefromparts(@p_anio, @p_mes, 28)
	  and (obf.fecha_fin is null or obf.fecha_fin >= datefromparts(@p_anio, @p_mes, 1))
	order by obf.dia_vencimiento;
end
go

CREATE OR ALTER PROCEDURE sp_reporte_progreso_ahorros
	@p_id_usuario int
as
begin
	select
		s.nombre_subcategoria as nombre_meta,
		pd.monto_mensual as ahorro_mensual,
		isnull(sum(t.monto_transaccion), 0) as monto_acumulado,
		case
			when pd.monto_mensual = 0 then 0
			else isnull(sum(t.monto_transaccion), 0) * 100.0 / 
				 (pd.monto_mensual * 12)
		end as porcentaje_completado,
		pd.monto_mensual * 12	as  monto_objetivo_anual
	from prespuesto_detalles pd
	inner join subcategorias s  
	on pd.id_subcategoria = s.id_subcategoria
	inner join categorias c     
	on s.id_categoria= c.id_categoria
	left join transacciones t   
	on t.id_detalle = pd.id_detalle
	and t.tipo_transaccion  = 'ahorro'
	where c.tipo_categoria = 'ahorro'
	  and pd.id_presupuesto in (
			select id_presupuesto from presupuestos
			where id_usuario = @p_id_usuario
		  )
	group by s.id_subcategoria, s.nombre_subcategoria, pd.monto_mensual
	order by porcentaje_completado desc;
end
go