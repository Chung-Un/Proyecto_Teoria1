CREATE OR ALTER PROCEDURE sp_login_usuario
    @p_correo   varchar(100),
    @p_password varchar(50)
as
begin
    select
        id_usuario,
        password,
        correo_electronico,
        primer_nombre,
        segundo_nombre,
        primer_apellido,
        segundo_apellido,
        fecha_ingreso,
        salario_mensual_base,
        estado_usuario
    from usuarios
    where correo_electronico = @p_correo
      AND password           = @p_password
      AND estado_usuario     = 1;
end
go