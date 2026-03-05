CREATE OR ALTER PROCEDURE sp_insertar_categoria
    @p_nombre_categoria varchar(300),
    @p_tipo_categoria varchar(100),
    @p_id_usuario int,
    @p_creado_por int
AS
BEGIN
    INSERT INTO categorias(
        nombre_categoria,
        tipo_categoria,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    )
    OUTPUT inserted.id_categoria, inserted.nombre_categoria
    VALUES(
        @p_nombre_categoria,
        @p_tipo_categoria,
        @p_creado_por,
        @p_creado_por,
        SYSDATETIME(),
        SYSDATETIME()
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_categoria
    @p_id_categoria int,
    @p_nombre_categoria varchar(300),
    @p_modificado_por int
AS 
BEGIN
    UPDATE categorias
    SET 
        nombre_categoria = @p_nombre_categoria,
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_categoria = @p_id_categoria;
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_categoria
    @p_id_categoria int,
    @p_modificado_por int
AS
BEGIN
    DECLARE @total_subcategorias int;

    SELECT @total_subcategorias = COUNT(*)
    FROM subcategorias
    WHERE id_categoria = @p_id_categoria;

    BEGIN
    IF 
        @total_subcategorias >=1
        RAISERROR('Elimine las subcategorias existentes antes de eliminar la categoria ',16,1);
        RETURN;
    END 

    DELETE
    FROM categorias
    WHERE id_categoria = @p_id_categoria;

    UPDATE categorias
    SET 
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_categoria = @p_id_categoria;
    
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_categoria
    @p_id_categoria int
AS
BEGIN
    SELECT 
        c.id_categoria,
        c.nombre_categoria,
        c.tipo_categoria,
        c.nombre_icono,
        c.color_hexademical,
        c.orden_presentacion,
        c.creado_por,
        c.modificado_por,
        c.creado_en,
        c.modificado_en
    FROM categorias c
    WHERE c.id_categoria = @p_id_categoria;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_categorias
    @p_id_usuario INT,
    @p_tipo VARCHAR(100)
AS
BEGIN
    SELECT 
        c.id_categoria,
        c.nombre_categoria,
        c.tipo_categoria,
        c.nombre_icono,
        c.color_hexademical,
        c.orden_presentacion
    FROM categorias c
END;
GO