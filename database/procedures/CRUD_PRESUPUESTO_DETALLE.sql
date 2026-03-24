CREATE OR ALTER PROCEDURE sp_insertar_presupuesto_detalle
	@p_id_presupuesto  int,
	@p_id_subcategoria int,
	@p_monto_mensual   decimal(12,2),
	@p_observaciones   varchar(500) = null,
	@p_creado_por      int
as
begin
	insert into prespuesto_detalles(
		id_presupuesto,
		id_subcategoria,
		monto_mensual,
		observaciones,
		creado_por,
		creado_en
	)
	values(
		@p_id_presupuesto,
		@p_id_subcategoria,
		@p_monto_mensual,
		@p_observaciones,
		@p_creado_por,
		getdate()
	);
end
go

CREATE OR ALTER PROCEDURE sp_actualizar_presupuesto_detalle
	@p_id_detalle     int,
	@p_monto_mensual  decimal(12,2),
	@p_observaciones  varchar(500),
	@p_modificado_por int
as
begin
	update prespuesto_detalles set
		monto_mensual  = @p_monto_mensual,
		observaciones  = @p_observaciones,
		modificado_por = @p_modificado_por,
		modificado_en  = getdate()
	where id_detalle = @p_id_detalle;
end
go

CREATE OR ALTER PROCEDURE sp_eliminar_presupuesto_detalle
	@p_id_detalle     int,
	@p_modificado_por int
as
begin
	delete from prespuesto_detalles
	where id_detalle = @p_id_detalle;
end
go

CREATE OR ALTER PROCEDURE sp_consultar_presupuesto_detalle
	@p_id_detalle int
as
begin
	select
		pd.id_detalle,
		pd.id_presupuesto,
		pd.id_subcategoria,
		pd.monto_mensual,
		pd.observaciones,
		pd.creado_por,
		pd.modificado_por,
		pd.creado_en,
		pd.modificado_en
	from prespuesto_detalles pd
	where pd.id_detalle = @p_id_detalle;
end
go

CREATE OR ALTER PROCEDURE sp_listar_detalles_presupuesto
	@p_id_presupuesto int
as
begin
	select
		pd.id_detalle,
		pd.id_presupuesto,
		pd.id_subcategoria,
		pd.monto_mensual,
		pd.observaciones,
		pd.creado_por,
		pd.modificado_por,
		pd.creado_en,
		pd.modificado_en
	from prespuesto_detalles pd
	where pd.id_presupuesto = @p_id_presupuesto;
end
go