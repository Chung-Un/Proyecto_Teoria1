CREATE OR ALTER PROCEDURE sp_insertar_usuario
	@p_primer_nombre varchar(100),
	@p_segundo_nombre varchar(100),
	@p_primer_apellido varchar(100),
	@p_segundo_apellido varchar(100),
	@p_email varchar(100),
	@p_salario_mensual decimal(12,2),
	@p_password varchar(50),
	@p_creado_por varchar(30)
AS
BEGIN
	INSERT INTO usuarios(
	password,correo_electronico,
	primer_nombre,segundo_nombre,
	primer_apellido,segundo_apellido,
	fecha_ingreso,salario_mensual_base,
	estado_usuario,creado_por,modificado_por,
	creado_en,modificado_en)
	OUTPUT inserted.id_usuario,inserted.correo_electronico
	VALUES(@p_password,@p_email,
			@p_primer_nombre,@p_segundo_nombre,
			@p_primer_apellido, @p_segundo_apellido,
			GETDATE(),@p_salario_mensual,
			1,@p_creado_por,
			@p_creado_por,GETDATE(),
			GETDATE()
			);

END;
GO--indica fin de bloque de instrucciones

CREATE OR ALTER PROCEDURE sp_actualizar_usuario
	@p_id_usuario int,
	@p_primer_nombre varchar(100),
	@p_segundo_nombre varchar(100),
	@p_primer_apellido varchar(100),
	@p_segundo_apellido varchar(100),
	@p_salario_mensual decimal(12,2)
AS 
BEGIN
	UPDATE usuarios
	SET primer_nombre = @p_primer_nombre,
		segundo_nombre = @p_segundo_nombre,
		primer_apellido = @p_primer_apellido,
		segundo_apellido = @p_segundo_apellido,
		salario_mensual_base = @p_salario_mensual,
		modificado_por = @p_id_usuario,
		modificado_en = SYSDATETIME()
	WHERE id_usuario = @p_id_usuario
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_usuario
	@p_id_usuario int
AS
BEGIN
	UPDATE usuarios
	SET estado_usuario = 0,
	modificado_en = SYSDATETIME(),
	modificado_por = @p_id_usuario
	WHERE id_usuario = @p_id_usuario
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_usuario
    @p_id_usuario INT
AS
BEGIN
    SELECT 
        id_usuario,
        password,
        correo_electronico,
        primer_nombre,
        segundo_nombre,
        primer_apellido,
        segundo_apellido,
        fecha_ingreso,
        salario_mensual_base,
        estado_usuario,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    FROM usuarios
    WHERE id_usuario = @p_id_usuario;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_usuarios
AS
BEGIN
    SELECT 
        id_usuario,
       CONCAT(primer_nombre,' ',primer_apellido) AS nombre_completo,
		correo_electronico,
		CASE
			WHEN estado_usuario =1 
			THEN 'Activo'
			ELSE 'Inactivo'
		END AS estado,
        fecha_ingreso,
        salario_mensual_base
    FROM usuarios
END;
GO