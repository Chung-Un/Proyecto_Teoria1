CREATE TABLE `usuarios` (
  `id_usuario` varchar(30) PRIMARY KEY,
  `password` varchar(50),
  `correo_electronico` varchar(100),
  `primer_nombre` varchar(100),
  `segundo_nombre` varchar(100),
  `primer_apellido` varchar(100),
  `segundo_apellido` varchar(100),
  `fecha_ingreso` datetime2,
  `salario_mensual_base` decimal(12,2),
  `estado_usuario` bool,
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `presupuestos` (
  `id_presupuesto` varchar(30) PRIMARY KEY,
  `id_usuario` varchar(30),
  `nombre_presupuesto` varchar(300),
  `anio_inicio` smallint,
  `mes_inicio` tinyint,
  `anio_fin` smallint,
  `mes_fin` tinyint,
  `total_ingresos_planificados` decimal(12,2),
  `total_gastos_planificados` decimal(12,2),
  `total_ahorro_planificado` decimal(12,2),
  `fecha_y_hora_creacion` datetime2,
  `estado_presupuesto` varchar(100),
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `categorias` (
  `id_categoria` varchar(30) PRIMARY KEY,
  `nombre_categoria` varchar(300),
  `tipo_categoria` varchar(100),
  `nombre_icono` varchar(100),
  `color_hexademical` varchar(7),
  `orden_presentacion` int,
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `subcategorias` (
  `id_subcategoria` varchar(30) PRIMARY KEY,
  `id_categoria` varchar(30),
  `nombre_subcategoria` varchar(300),
  `estado_subcategoria` bool,
  `subcategoria_por_defecto` bool,
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `prespuesto_detalles` (
  `id_detalle` varchar(30) PRIMARY KEY,
  `id_presupuesto` varchar(30),
  `id_subcategoria` varchar(30),
  `monto_mensual` decimal(12,2),
  `observaciones` varchar(500),
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `obligaciones_fijas` (
  `id_obligacion` varchar(30) PRIMARY KEY,
  `id_subcategoria` varchar(30),
  `nombre_obligacion` varchar(300),
  `monto_mensual` decimal(12,2),
  `dia_vencimiento` tinyint,
  `estado_vigente` bool,
  `fecha_inicio` date,
  `fecha_fin` date,
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `transacciones` (
  `id_transaccion` varchar(30) PRIMARY KEY,
  `id_usuario` varchar(30),
  `id_presupuesto_detalle` varchar(30),
  `anio_transaccion` smallint,
  `mes_transaccion` tinyint,
  `id_subcategoria` varchar(30),
  `tipo_transaccionn` varchar(100),
  `descripcion_movimiento` varchar(300),
  `monto_transaccion` decimal(12,2),
  `fecha_transaccion` date,
  `metodo_pago` varchar(100),
  `numero_factura` int,
  `observaciones` varchar(500),
  `fecha_y_hora_registro` datetime2,
  `creado_por` varchar(30),
  `modificado_por` varchar(30),
  `creado_en` datetime2,
  `modificado_en` datetime2
);

CREATE TABLE `transacciones_obligaciones_fijas` (
  `id_transaccion` varchar(30) PRIMARY KEY,
  `id_obligacion` varchar(30)
);

ALTER TABLE `presupuestos` ADD FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`);

ALTER TABLE `prespuesto_detalles` ADD FOREIGN KEY (`id_presupuesto`) REFERENCES `presupuestos` (`id_presupuesto`);

ALTER TABLE `prespuesto_detalles` ADD FOREIGN KEY (`id_presupuesto`) REFERENCES `transacciones` (`id_presupuesto_detalle`);

ALTER TABLE `transacciones_obligaciones_fijas` ADD FOREIGN KEY (`id_transaccion`) REFERENCES `transacciones` (`id_transaccion`);

ALTER TABLE `transacciones_obligaciones_fijas` ADD FOREIGN KEY (`id_obligacion`) REFERENCES `obligaciones_fijas` (`id_obligacion`);

ALTER TABLE `subcategorias` ADD FOREIGN KEY (`id_categoria`) REFERENCES `categorias` (`id_categoria`);

ALTER TABLE `obligaciones_fijas` ADD FOREIGN KEY (`id_subcategoria`) REFERENCES `subcategorias` (`id_subcategoria`);

ALTER TABLE `prespuesto_detalles` ADD FOREIGN KEY (`id_subcategoria`) REFERENCES `subcategorias` (`id_subcategoria`);
