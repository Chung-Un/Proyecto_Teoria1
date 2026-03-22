CREATE OR ALTER TRIGGER tgr_crear_subcategoria_defecto
on categorias
after insert
as 
begin 
	insert into subcategorias(
        id_categoria,
        nombre_subcategoria,
        estado_subcategoria,
        subcategoria_por_defecto,
        creado_por,
        modificado_por,
        creado_en,
        modificado_en
    )

    select
     i.id_categoria,
        'General',      
        1,              
        1,             
        i.creado_por,
        i.creado_por,
        GETDATE(),
        GETDATE()
    from inserted i;   
end
go
        
