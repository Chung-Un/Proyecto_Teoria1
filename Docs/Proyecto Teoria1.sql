CREATE TABLE [usuarios] (
  [id_usuario] int PRIMARY KEY,
  [password] varchar(50),
  [correo_electronico] varchar(100),
  [primer_nombre] varchar(100),
  [segundo_nombre] varchar(100),
  [primer_apellido] varchar(100),
  [segundo_apellido] varchar(100),
  [fecha_ingreso] datetime2,
  [salario_mensual_base] decimal(12,2),
  [estado_usuario] bit,
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [presupuestos] (
  [id_presupuesto] int PRIMARY KEY,
  [id_usuario] int,
  [nombre_presupuesto] varchar(300),
  [anio_inicio] smallint,
  [mes_inicio] tinyint,
  [anio_fin] smallint,
  [mes_fin] tinyint,
  [total_ingresos_planificados] decimal(12,2),
  [total_gastos_planificados] decimal(12,2),
  [total_ahorro_planificado] decimal(12,2),
  [fecha_y_hora_creacion] datetime2,
  [estado_presupuesto] varchar(100),
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [categorias] (
  [id_categoria] int PRIMARY KEY,
  [nombre_categoria] varchar(300),
  [tipo_categoria] varchar(100),
  [nombre_icono] varchar(100),
  [color_hexademical] varchar(7),
  [orden_presentacion] int,
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [subcategorias] (
  [id_subcategoria] int PRIMARY KEY,
  [id_categoria] int,
  [nombre_subcategoria] varchar(300),
  [estado_subcategoria] bit,
  [subcategoria_por_defecto] bit,
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [prespuesto_detalles] (
  [id_detalle] int PRIMARY KEY,
  [id_presupuesto] int,
  [id_subcategoria] int,
  [monto_mensual] decimal(12,2),
  [observaciones] varchar(500),
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [obligaciones_fijas] (
  [id_obligacion] int PRIMARY KEY,
  [id_subcategoria] int,
  [nombre_obligacion] varchar(300),
  [monto_mensual] decimal(12,2),
  [dia_vencimiento] tinyint,
  [estado_vigente] bit,
  [fecha_inicio] date,
  [fecha_fin] date,
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [transacciones] (
  [id_transaccion] int PRIMARY KEY,
  [id_usuario] int,
  [id_detalle] int,
  [anio_transaccion] smallint,
  [mes_transaccion] tinyint,
  [id_subcategoria] int,
  [tipo_transaccion] varchar(100),
  [descripcion_movimiento] varchar(300),
  [monto_transaccion] decimal(12,2),
  [fecha_transaccion] date,
  [metodo_pago] varchar(100),
  [numero_factura] int,
  [observaciones] varchar(500),
  [fecha_y_hora_registro] datetime2,
  [creado_por] varchar(30),
  [modificado_por] varchar(30),
  [creado_en] datetime2,
  [modificado_en] datetime2
)
GO

CREATE TABLE [transacciones_obligaciones_fijas] (
  [id_transaccion] int PRIMARY KEY,
  [id_obligacion] int
)
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'usuarios',
@level2type = N'Column', @level2name = 'id_usuario';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'presupuestos',
@level2type = N'Column', @level2name = 'id_presupuesto';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'categorias',
@level2type = N'Column', @level2name = 'id_categoria';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'subcategorias',
@level2type = N'Column', @level2name = 'id_subcategoria';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'prespuesto_detalles',
@level2type = N'Column', @level2name = 'id_detalle';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'obligaciones_fijas',
@level2type = N'Column', @level2name = 'id_obligacion';
GO

EXEC sp_addextendedproperty
@name = N'Column_Description',
@value = 'IDENTITY(1,1)',
@level0type = N'Schema', @level0name = 'dbo',
@level1type = N'Table',  @level1name = 'transacciones',
@level2type = N'Column', @level2name = 'id_transaccion';
GO

ALTER TABLE [presupuestos] ADD FOREIGN KEY ([id_usuario]) REFERENCES [usuarios] ([id_usuario])
GO

ALTER TABLE [prespuesto_detalles] ADD FOREIGN KEY ([id_presupuesto]) REFERENCES [presupuestos] ([id_presupuesto])
GO

ALTER TABLE [transacciones_obligaciones_fijas] ADD FOREIGN KEY ([id_transaccion]) REFERENCES [transacciones] ([id_transaccion])
GO

ALTER TABLE [transacciones_obligaciones_fijas] ADD FOREIGN KEY ([id_obligacion]) REFERENCES [obligaciones_fijas] ([id_obligacion])
GO

ALTER TABLE [subcategorias] ADD FOREIGN KEY ([id_categoria]) REFERENCES [categorias] ([id_categoria])
GO

ALTER TABLE [obligaciones_fijas] ADD FOREIGN KEY ([id_subcategoria]) REFERENCES [subcategorias] ([id_subcategoria])
GO

ALTER TABLE [prespuesto_detalles] ADD FOREIGN KEY ([id_subcategoria]) REFERENCES [subcategorias] ([id_subcategoria])
GO

ALTER TABLE [transacciones] ADD FOREIGN KEY ([id_detalle]) REFERENCES [prespuesto_detalles] ([id_detalle])
GO
