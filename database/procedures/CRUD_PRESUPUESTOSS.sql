CREATE OR ALTER PROCEDURE sp_insertar_presupuesto
	@p_id_usuario int,
	@p_nombre_presupuesto varchar(200),
	@p_anio_inicio smallint,
	@p_mes_inicio tinyint,
	@p_mes_fin tinyint,
	@p_anio_fin smallint,
	@p_creado_por int = null,
	@p_id_presupuesto int output
as
begin
	insert into presupuestos(
		id_usuario, 
        nombre_presupuesto, 
        anio_inicio, 
        mes_inicio,
		mes_fin, 
        anio_fin, 
        estado_presupuesto, 
        creado_por, 
        creado_en
	)
	values(
		@p_id_usuario, 
        @p_nombre_presupuesto, 
        @p_anio_inicio, 
        @p_mes_inicio,
		@p_mes_fin, 
        @p_anio_fin, 
        'activo',
		isnull(@p_creado_por, @p_id_usuario),
        getdate()
	);

	set @p_id_presupuesto = scope_identity();
end
go
CREATE OR ALTER PROCEDURE sp_actualizar_presupuesto
	@p_id_presupuesto int,
	@p_nombre_presupuesto varchar(200),
	@p_anio_inicio smallint,
	@p_mes_inicio tinyint,
	@p_mes_fin tinyint,
	@p_anio_fin smallint,
	@p_modificado_por int
as
begin
	update presupuestos set
		nombre_presupuesto = @p_nombre_presupuesto,
		anio_inicio = @p_anio_inicio,
		mes_inicio = @p_mes_inicio,
		mes_fin = @p_mes_fin,
		anio_fin= @p_anio_fin,
		modificado_por= @p_modificado_por,
		modificado_en= getdate()
	where id_presupuesto = @p_id_presupuesto;
end
go

CREATE OR ALTER PROCEDURE sp_eliminar_presupuesto
    @p_id_presupuesto int,
    @p_modificado_por int
AS
BEGIN

    IF
    EXISTS (select 1 
            from transacciones t 
            INNER JOIN prespuesto_detalles pd ON 
            t.id_detalle = pd.id_detalle
            where pd.id_presupuesto = @p_id_presupuesto)
        BEGIN 
        RAISERROR('El presupuesto tiene transacciones asociadas',16,1);
        RETURN;
        END
    
UPDATE presupuestos
SET
    estado_presupuesto= 0,
    modificado_por = @p_modificado_por,
    modificado_en = SYSDATETIME()
WHERE id_presupuesto = @p_id_presupuesto
    
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_presupuesto
    @p_id_presupuesto int
AS
BEGIN 
    SELECT
       p.id_presupuesto,
       p.nombre_presupuesto,
       p.anio_inicio,
       p.anio_fin,
       p.mes_fin,
       p.mes_inicio,
       p.total_ingresos_planificados,
       p.total_gastos_planificados,
       p.total_ahorro_planificado,      
       p.fecha_y_hora_creacion,
       p.estado_presupuesto,
       p.creado_por,
       p.modificado_por,
       p.creado_en,
       p.modificado_en
    FROM presupuestos p
    WHERE p.id_presupuesto = @p_id_presupuesto
END
GO

CREATE OR ALTER PROCEDURE sp_listar_presupuestos_usuario
    @p_id_usuario int,
    @p_estado     varchar(100) = NULL
AS
BEGIN
    SELECT
        p.id_presupuesto,
        p.nombre_presupuesto,
        p.anio_inicio,
        p.anio_fin,
        p.mes_fin,
        p.mes_inicio,
        p.total_ingresos_planificados,
        p.total_gastos_planificados,
        p.total_ahorro_planificado,     
        p.fecha_y_hora_creacion,
        p.estado_presupuesto,
        p.creado_por,
        p.modificado_por,
        p.creado_en,
        p.modificado_en
    FROM presupuestos p
    WHERE p.id_usuario = @p_id_usuario
      AND (@p_estado IS NULL OR p.estado_presupuesto = @p_estado);
END
GO

CREATE OR ALTER PROCEDURE sp_listar_todos_presupuestos
    @p_estado varchar(100) = NULL
AS
BEGIN
    SELECT
        p.id_presupuesto,
        p.nombre_presupuesto,
        p.anio_inicio,
        p.anio_fin,
        p.mes_fin,
        p.mes_inicio,
        p.total_ingresos_planificados,
        p.total_gastos_planificados,
        p.total_ahorro_planificado,
        p.fecha_y_hora_creacion,
        p.estado_presupuesto,
        p.creado_por,
        p.modificado_por,
        p.creado_en,
        p.modificado_en
    FROM presupuestos p
    WHERE (@p_estado IS NULL OR p.estado_presupuesto = @p_estado);
END
GO