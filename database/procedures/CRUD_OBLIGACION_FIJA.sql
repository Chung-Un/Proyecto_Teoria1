CREATE OR ALTER PROCEDURE sp_insertar_obligacion
	@p_id_usuario int,
	@p_id_subcategoria int,
	@p_nombre varchar(200),
	@p_monto decimal(12,2),
	@p_dia_vencimiento tinyint,
	@p_fecha_inicio date,
	@p_fecha_fin date = null,
	@p_creado_por int
as
begin
	insert into obligaciones_fijas(
		creado_por, 
        id_subcategoria, 
        nombre_obligacion,
		monto_mensual, 
        dia_vencimiento, 
        fecha_inicio, 
        fecha_fin,
		estado_vigente, 
        creado_en
	)
	values(
		@p_id_usuario, 
        @p_id_subcategoria, 
        @p_nombre,
		@p_monto,
        @p_dia_vencimiento, 
        @p_fecha_inicio, 
        @p_fecha_fin,
		1, 
        getdate()
	);
end
go

CREATE OR ALTER PROCEDURE sp_actualizar_obligacion
	@p_id_obligacion int,
	@p_nombre varchar(200),
	@p_monto decimal(12,2),
	@p_dia_vencimiento tinyint,
	@p_fecha_fin date = null,
	@p_activo bit,
	@p_modificado_por  int
as
begin
	update obligaciones_fijas set
		nombre_obligacion = @p_nombre,
		monto_mensual = @p_monto,
		dia_vencimiento = @p_dia_vencimiento,
		fecha_fin = @p_fecha_fin,
		estado_vigente= @p_activo,
		modificado_por= @p_modificado_por,
		modificado_en= getdate()
	where id_obligacion = @p_id_obligacion;
end
go

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
as
begin
	select
		id_obligacion,
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
	from obligaciones_fijas
	where id_obligacion = @p_id_obligacion;
end
go

CREATE OR ALTER PROCEDURE sp_listar_obligaciones_usuario
	@p_id_usuario int,
	@p_activo bit
as
begin
	select
		o.id_obligacion,
		o.id_subcategoria,
		o.nombre_obligacion,
		o.monto_mensual,
		o.dia_vencimiento,
		o.estado_vigente,
		o.fecha_inicio,
		o.fecha_fin,
		o.creado_por,
		o.modificado_por,
		o.creado_en,
		o.modificado_en
	from obligaciones_fijas o
	where o.creado_por    = @p_id_usuario
	  and o.estado_vigente = @p_activo;
end
go