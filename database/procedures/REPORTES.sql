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