CREATE OR ALTER PROCEDURE sp_insertar_subcategoria
    @p_id_subcategoria int,
    @p_nombre_subcategoria varchar(100),
    @p_descripcion varchar(500),
    @p_es_defecto bit,
    @p_creado_por int
AS
BEGIN
    INSERT INTO subcategorias(
        nombre_subcategoria,
        estado_subcategoria,
        subcategoria_por_defecto,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    )
    VALUES(
        @p_nombre_subcategoria,
        1,
        @p_es_defecto,
        @p_creado_por,
        @p_creado_por,
        SYSDATETIME(),
        SYSDATETIME()
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_actualizar_subcategoria
    @p_id_subcategoria int,
    @p_nombre_subcategoria varchar,
    @p_descripcion varchar,
    @p_modificado_por int
AS 
BEGIN
    UPDATE subcategorias
    SET 
        nombre_subcategoria = @p_nombre_subcategoria,
        descripcion = @p_descripcion,
        modificado_por = @p_modificado_por,
        modificado_en = SYSDATETIME()
    WHERE id_subcategoria = @p_id_subcategoria;
END;
GO

CREATE OR ALTER PROCEDURE sp_eliminar_subcategoria
    @p_id_subcategoria int,
    @p_modificado_por int
AS
BEGIN

    IF
    EXISTS (select 1 
            from prespuesto_detalles 
            where id_subcategoria = @p_id_subcategoria)
    OR EXISTS(select 1 
              from transacciones 
              where id_subcategoria = @p_id_subcategoria)
    OR EXISTS(select 1 
              from obligaciones_fijas 
              where id_subcategoria = @p_id_subcategoria)
        BEGIN 
        RAISERROR('Subcategoria esta en uso',16,1);
        RETURN;
        END
    
UPDATE subcategorias
SET
    estado_subcategoria= 0,
    modificado_por = @p_modificado_por,
    modificado_en = SYSDATETIME()
WHERE id_subcategoria = @p_id_subcategoria
    
    
END;
GO

CREATE OR ALTER PROCEDURE sp_consultar_subcategoria
    @p_id_subcategoria int
AS
BEGIN
    SELECT 
        s.id_categoria,
        s.id_subcategoria,
        s.nombre_subcategoria,
        s.estado_subcategoria,
        s.subcategoria_por_defecto,
        s.creado_en,
        s.creado_por,
        s.modificado_en,
        s.modificado_por
    FROM subcategorias s
    WHERE s.id_subcategoria = @p_id_subcategoria;
END;
GO

CREATE OR ALTER PROCEDURE sp_listar_subcategorias_por_categoria
    @p_id_categoria INT
AS
BEGIN
    SELECT
        s.id_categoria,
        s.id_subcategoria,
        s.nombre_subcategoria,
        s.estado_subcategoria,
        s.subcategoria_por_defecto,
        s.creado_en,
        s.creado_por,
        s.modificado_en,
        s.modificado_por
    FROM subcategorias s
    INNER JOIN categorias c ON
    s.id_categoria = @p_id_categoria;

END;
GO