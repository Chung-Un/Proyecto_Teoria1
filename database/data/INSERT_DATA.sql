INSERT INTO usuarios (password, correo_electronico, primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, fecha_ingreso, salario_mensual_base, estado_usuario, creado_por, modificado_por, creado_en, modificado_en)
VALUES
('ADMIN@2026!',      'admin@presupuesto.hn',     'Carlos',    'Alberto',  'Martinez',  'Lopez',    GETDATE(), 25000.00, 1, 1, 1, GETDATE(), GETDATE()),
-- Usuarios regulares
('Pass@1234',        'ana.garcia@gmail.com',      'Ana',       'Maria',    'Garcia',    'Reyes',    GETDATE(), 18000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'jose.hernandez@gmail.com',  'Jose',      'Luis',     'Hernandez', 'Mejia',    GETDATE(), 22000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'maria.rodriguez@gmail.com', 'Maria',     'Elena',    'Rodriguez', 'Castro',   GETDATE(), 15000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'pedro.lopez@gmail.com',     'Pedro',     'Antonio',  'Lopez',     'Flores',   GETDATE(), 20000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'lucia.martinez@gmail.com',  'Lucia',     'Isabel',   'Martinez',  'Diaz',     GETDATE(), 17500.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'diego.flores@gmail.com',    'Diego',     'Alejandro','Flores',    'Zuniga',   GETDATE(), 19000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'sofia.castro@gmail.com',    'Sofia',     'Valentina','Castro',    'Morales',  GETDATE(), 16000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'andres.mejia@gmail.com',    'Andres',    'Felipe',   'Mejia',     'Aguilar',  GETDATE(), 23000.00, 1, 1, 1, GETDATE(), GETDATE()),
('Pass@1234',        'laura.zuniga@gmail.com',    'Laura',     'Patricia', 'Zuniga',    'Pineda',   GETDATE(), 14000.00, 1, 1, 1, GETDATE(), GETDATE());
GO
 

-Categorias
-- TIPO: ingreso
INSERT INTO categorias (nombre_categoria, tipo_categoria, nombre_icono, color_hexademical, orden_presentacion, creado_por, modificado_por, creado_en, modificado_en)
VALUES
('Salario Principal',   'ingreso', 'wallet',      '#27AE60', 1, 1, 1, GETDATE(), GETDATE()),
('Ingresos Adicionales','ingreso', 'plus-circle',  '#2ECC71', 2, 1, 1, GETDATE(), GETDATE()),
('Bonificaciones',      'ingreso', 'gift',         '#1ABC9C', 3, 1, 1, GETDATE(), GETDATE()),
('Freelance',           'ingreso', 'briefcase',    '#16A085', 4, 1, 1, GETDATE(), GETDATE());
GO
 
-- TIPO: gasto
INSERT INTO categorias (nombre_categoria, tipo_categoria, nombre_icono, color_hexademical, orden_presentacion, creado_por, modificado_por, creado_en, modificado_en)
VALUES
('Alimentacion',        'gasto', 'shopping-cart', '#E74C3C', 5,  1, 1, GETDATE(), GETDATE()),
('Servicios Publicos',  'gasto', 'zap',           '#E67E22', 6,  1, 1, GETDATE(), GETDATE()),
('Transporte',          'gasto', 'truck',         '#F39C12', 7,  1, 1, GETDATE(), GETDATE()),
('Educacion',           'gasto', 'book',          '#8E44AD', 8,  1, 1, GETDATE(), GETDATE()),
('Salud',               'gasto', 'heart',         '#C0392B', 9,  1, 1, GETDATE(), GETDATE()),
('Entretenimiento',     'gasto', 'music',         '#2980B9', 10, 1, 1, GETDATE(), GETDATE()),
('Vivienda',            'gasto', 'home',          '#7F8C8D', 11, 1, 1, GETDATE(), GETDATE()),
('Vestuario',           'gasto', 'tag',           '#D35400', 12, 1, 1, GETDATE(), GETDATE()),
('Seguros',             'gasto', 'shield',        '#95A5A6', 13, 1, 1, GETDATE(), GETDATE());
GO
 
-- TIPO: ahorro
INSERT INTO categorias (nombre_categoria, tipo_categoria, nombre_icono, color_hexademical, orden_presentacion, creado_por, modificado_por, creado_en, modificado_en)
VALUES
('Fondo de Emergencia', 'ahorro', 'umbrella',    '#3498DB', 14, 1, 1, GETDATE(), GETDATE()),
('Inversiones',         'ahorro', 'trending-up', '#2471A3', 15, 1, 1, GETDATE(), GETDATE()),
('Metas Especificas',   'ahorro', 'target',      '#1F618D', 16, 1, 1, GETDATE(), GETDATE()),
('Provisiones',         'ahorro', 'archive',     '#154360', 17, 1, 1, GETDATE(), GETDATE());
GO

-- Subcategorias adicionales
INSERT INTO subcategorias (id_categoria, nombre_subcategoria, estado_subcategoria, subcategoria_por_defecto, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(1,  'Salario Base',         1, 1, 1, 1, GETDATE(), GETDATE()),
(2,  'Horas Extra',          1, 0, 1, 1, GETDATE(), GETDATE()),
(2,  'Comisiones',           1, 0, 1, 1, GETDATE(), GETDATE()),
(2,  'Ventas',               1, 0, 1, 1, GETDATE(), GETDATE()),
(3,  'Bono Productividad',   1, 0, 1, 1, GETDATE(), GETDATE()),
(3,  'Bono Navidad',         1, 0, 1, 1, GETDATE(), GETDATE()),
(4,  'Diseno Grafico',       1, 0, 1, 1, GETDATE(), GETDATE()),
(4,  'Desarrollo Web',       1, 0, 1, 1, GETDATE(), GETDATE()),
(4,  'Consultoria',          1, 0, 1, 1, GETDATE(), GETDATE()),
(5,  'Supermercado',         1, 0, 1, 1, GETDATE(), GETDATE()),
(5,  'Restaurantes',         1, 0, 1, 1, GETDATE(), GETDATE()),
(5,  'Comida Rapida',        1, 0, 1, 1, GETDATE(), GETDATE()),
(5,  'Cafeterias',           1, 0, 1, 1, GETDATE(), GETDATE()),
(6,  'Energia Electrica',    1, 0, 1, 1, GETDATE(), GETDATE()),
(6,  'Agua',                 1, 0, 1, 1, GETDATE(), GETDATE()),
(6,  'Gas',                  1, 0, 1, 1, GETDATE(), GETDATE()),
(6,  'Internet',             1, 0, 1, 1, GETDATE(), GETDATE()),
(6,  'Telefono',             1, 0, 1, 1, GETDATE(), GETDATE()),
(7,  'Combustible',          1, 0, 1, 1, GETDATE(), GETDATE()),
(7,  'Transporte Publico',   1, 0, 1, 1, GETDATE(), GETDATE()),
(7,  'Mantenimiento Vehiculo',1,0, 1, 1, GETDATE(), GETDATE()),
(7,  'Estacionamiento',      1, 0, 1, 1, GETDATE(), GETDATE()),
(8,  'Colegiatura',          1, 0, 1, 1, GETDATE(), GETDATE()),
(8,  'Libros',               1, 0, 1, 1, GETDATE(), GETDATE()),
(8,  'Material Escolar',     1, 0, 1, 1, GETDATE(), GETDATE()),
(8,  'Cursos Online',        1, 0, 1, 1, GETDATE(), GETDATE()),
(9,  'Medicamentos',         1, 0, 1, 1, GETDATE(), GETDATE()),
(9,  'Consultas',            1, 0, 1, 1, GETDATE(), GETDATE()),
(9,  'Examenes',             1, 0, 1, 1, GETDATE(), GETDATE()),
(10, 'Cine',                 1, 0, 1, 1, GETDATE(), GETDATE()),
(10, 'Streaming',            1, 0, 1, 1, GETDATE(), GETDATE()),
(10, 'Deportes',             1, 0, 1, 1, GETDATE(), GETDATE()),
(10, 'Eventos',              1, 0, 1, 1, GETDATE(), GETDATE()),
(11, 'Alquiler',             1, 0, 1, 1, GETDATE(), GETDATE()),
(11, 'Mantenimiento',        1, 0, 1, 1, GETDATE(), GETDATE()),
(11, 'Articulos del Hogar',  1, 0, 1, 1, GETDATE(), GETDATE()),
(12, 'Ropa',                 1, 0, 1, 1, GETDATE(), GETDATE()),
(12, 'Calzado',              1, 0, 1, 1, GETDATE(), GETDATE()),
(12, 'Accesorios',           1, 0, 1, 1, GETDATE(), GETDATE()),
(13, 'Seguro Vida',          1, 0, 1, 1, GETDATE(), GETDATE()),
(13, 'Seguro Vehiculo',      1, 0, 1, 1, GETDATE(), GETDATE()),
(13, 'Seguro Medico',        1, 0, 1, 1, GETDATE(), GETDATE()),
(14, 'Reserva Mensual',      1, 0, 1, 1, GETDATE(), GETDATE()),
(14, 'Fondo Medico',         1, 0, 1, 1, GETDATE(), GETDATE()),
(15, 'Acciones',             1, 0, 1, 1, GETDATE(), GETDATE()),
(15, 'Criptomonedas',        1, 0, 1, 1, GETDATE(), GETDATE()),
(15, 'Fondos Mutuos',        1, 0, 1, 1, GETDATE(), GETDATE()),
(16, 'Vacaciones',           1, 0, 1, 1, GETDATE(), GETDATE()),
(16, 'Vehiculo',             1, 0, 1, 1, GETDATE(), GETDATE()),
(16, 'Casa Propia',          1, 0, 1, 1, GETDATE(), GETDATE()),
(17, 'Reparaciones',         1, 0, 1, 1, GETDATE(), GETDATE()),
(17, 'Imprevistos',          1, 0, 1, 1, GETDATE(), GETDATE());
GO

--Presupuestos
INSERT INTO presupuestos (id_usuario, nombre_presupuesto, anio_inicio, mes_inicio, anio_fin, mes_fin, total_ingresos_planificados, total_gastos_planificados, total_ahorro_planificado, fecha_y_hora_creacion, estado_presupuesto, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(1,  'Presupuesto Ene-Feb 2026 - Carlos',  2026, 1, 2026, 2, 25000.00, 18000.00, 4000.00, GETDATE(), 'activo', 1,  1,  GETDATE(), GETDATE()),
(2,  'Presupuesto Ene-Feb 2026 - Ana',     2026, 1, 2026, 2, 18000.00, 13000.00, 3000.00, GETDATE(), 'activo', 2,  2,  GETDATE(), GETDATE()),
(3,  'Presupuesto Ene-Feb 2026 - Jose',    2026, 1, 2026, 2, 22000.00, 16000.00, 3500.00, GETDATE(), 'activo', 3,  3,  GETDATE(), GETDATE()),
(4,  'Presupuesto Ene-Feb 2026 - Maria',   2026, 1, 2026, 2, 15000.00, 11000.00, 2000.00, GETDATE(), 'activo', 4,  4,  GETDATE(), GETDATE()),
(5,  'Presupuesto Ene-Feb 2026 - Pedro',   2026, 1, 2026, 2, 20000.00, 14500.00, 3000.00, GETDATE(), 'activo', 5,  5,  GETDATE(), GETDATE()),
(6,  'Presupuesto Ene-Feb 2026 - Lucia',   2026, 1, 2026, 2, 17500.00, 12500.00, 2500.00, GETDATE(), 'activo', 6,  6,  GETDATE(), GETDATE()),
(7,  'Presupuesto Ene-Feb 2026 - Diego',   2026, 1, 2026, 2, 19000.00, 13500.00, 3000.00, GETDATE(), 'activo', 7,  7,  GETDATE(), GETDATE()),
(8,  'Presupuesto Ene-Feb 2026 - Sofia',   2026, 1, 2026, 2, 16000.00, 11500.00, 2500.00, GETDATE(), 'activo', 8,  8,  GETDATE(), GETDATE()),
(9,  'Presupuesto Ene-Feb 2026 - Andres',  2026, 1, 2026, 2, 23000.00, 16500.00, 3500.00, GETDATE(), 'activo', 9,  9,  GETDATE(), GETDATE()),
(10, 'Presupuesto Ene-Feb 2026 - Laura',   2026, 1, 2026, 2, 14000.00, 10000.00, 2000.00, GETDATE(), 'activo', 10, 10, GETDATE(), GETDATE());
GO

--Presupuestos detalles
--subcategorias
-- 18=Salario Base, 27=Supermercado, 28=Restaurantes
-- 31=Energia Electrica, 32=Agua, 34=Internet
-- 36=Combustible, 51=Alquiler, 57=Seguro Vida
-- 60=Reserva Mensual, 65=Vacaciones

-- Carlos (presupuesto 1)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(1, 18, 25000.00, 'Salario mensual base',             1, 1, GETDATE(), GETDATE()),  -- detalle 1
(1, 27,  4000.00, 'Compras semanales supermercado',   1, 1, GETDATE(), GETDATE()),  -- detalle 2
(1, 28,  1500.00, 'Salidas a restaurantes',           1, 1, GETDATE(), GETDATE()),  -- detalle 3
(1, 31,   800.00, 'Recibo energia electrica',         1, 1, GETDATE(), GETDATE()),  -- detalle 4
(1, 32,   300.00, 'Recibo agua',                      1, 1, GETDATE(), GETDATE()),  -- detalle 5
(1, 34,   700.00, 'Internet hogar',                   1, 1, GETDATE(), GETDATE()),  -- detalle 6
(1, 36,  2000.00, 'Combustible vehiculo',             1, 1, GETDATE(), GETDATE()),  -- detalle 7
(1, 51,  5000.00, 'Alquiler apartamento',             1, 1, GETDATE(), GETDATE()),  -- detalle 8
(1, 57,   600.00, 'Seguro de vida mensual',           1, 1, GETDATE(), GETDATE()),  -- detalle 9
(1, 60,  2000.00, 'Aporte mensual fondo emergencia',  1, 1, GETDATE(), GETDATE()),  -- detalle 10
(1, 65,  2000.00, 'Ahorro para vacaciones',           1, 1, GETDATE(), GETDATE());  -- detalle 11
GO
 
-- Ana (presupuesto 2)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(2, 18, 18000.00, 'Salario mensual',       2, 2, GETDATE(), GETDATE()),  -- detalle 12
(2, 27,  3500.00, 'Supermercado mensual',  2, 2, GETDATE(), GETDATE()),  -- detalle 13
(2, 31,   700.00, 'Energia electrica',     2, 2, GETDATE(), GETDATE()),  -- detalle 14
(2, 34,   600.00, 'Internet',              2, 2, GETDATE(), GETDATE()),  -- detalle 15
(2, 51,  4500.00, 'Alquiler',             2, 2, GETDATE(), GETDATE()),  -- detalle 16
(2, 60,  1500.00, 'Fondo emergencia',      2, 2, GETDATE(), GETDATE()),  -- detalle 17
(2, 65,  1500.00, 'Meta vacaciones',       2, 2, GETDATE(), GETDATE());  -- detalle 18
GO
 
-- Jose (presupuesto 3)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(3, 18, 22000.00, 'Salario mensual',   3, 3, GETDATE(), GETDATE()),  -- detalle 19
(3, 27,  4500.00, 'Supermercado',      3, 3, GETDATE(), GETDATE()),  -- detalle 20
(3, 28,  2000.00, 'Restaurantes',      3, 3, GETDATE(), GETDATE()),  -- detalle 21
(3, 36,  2500.00, 'Combustible',       3, 3, GETDATE(), GETDATE()),  -- detalle 22
(3, 51,  5500.00, 'Alquiler',         3, 3, GETDATE(), GETDATE()),  -- detalle 23
(3, 57,   700.00, 'Seguro vida',       3, 3, GETDATE(), GETDATE()),  -- detalle 24
(3, 60,  2000.00, 'Fondo emergencia',  3, 3, GETDATE(), GETDATE()),  -- detalle 25
(3, 65,  1500.00, 'Meta vacaciones',   3, 3, GETDATE(), GETDATE());  -- detalle 26
GO
 
-- Maria (presupuesto 4)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(4, 18, 15000.00, 'Salario mensual',   4, 4, GETDATE(), GETDATE()),  -- detalle 27
(4, 27,  3000.00, 'Supermercado',      4, 4, GETDATE(), GETDATE()),  -- detalle 28
(4, 31,   600.00, 'Energia electrica', 4, 4, GETDATE(), GETDATE()),  -- detalle 29
(4, 51,  4000.00, 'Alquiler',         4, 4, GETDATE(), GETDATE()),  -- detalle 30
(4, 60,  1000.00, 'Fondo emergencia',  4, 4, GETDATE(), GETDATE()),  -- detalle 31
(4, 65,  1000.00, 'Meta vacaciones',   4, 4, GETDATE(), GETDATE());  -- detalle 32
GO
 
-- Pedro (presupuesto 5)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(5, 18, 20000.00, 'Salario mensual',   5, 5, GETDATE(), GETDATE()),  -- detalle 33
(5, 27,  4000.00, 'Supermercado',      5, 5, GETDATE(), GETDATE()),  -- detalle 34
(5, 36,  2000.00, 'Combustible',       5, 5, GETDATE(), GETDATE()),  -- detalle 35
(5, 51,  4500.00, 'Alquiler',         5, 5, GETDATE(), GETDATE()),  -- detalle 36
(5, 57,   600.00, 'Seguro vida',       5, 5, GETDATE(), GETDATE()),  -- detalle 37
(5, 60,  1500.00, 'Fondo emergencia',  5, 5, GETDATE(), GETDATE()),  -- detalle 38
(5, 65,  1500.00, 'Meta vacaciones',   5, 5, GETDATE(), GETDATE());  -- detalle 39
GO
 
-- Lucia (presupuesto 6)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(6, 18, 17500.00, 'Salario',          6, 6, GETDATE(), GETDATE()),  -- detalle 40
(6, 27,  3200.00, 'Supermercado',     6, 6, GETDATE(), GETDATE()),  -- detalle 41
(6, 51,  4200.00, 'Alquiler',        6, 6, GETDATE(), GETDATE()),  -- detalle 42
(6, 60,  1300.00, 'Fondo emergencia', 6, 6, GETDATE(), GETDATE());  -- detalle 43
GO
 
-- Diego (presupuesto 7)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(7, 18, 19000.00, 'Salario',          7, 7, GETDATE(), GETDATE()),  -- detalle 44
(7, 27,  3800.00, 'Supermercado',     7, 7, GETDATE(), GETDATE()),  -- detalle 45
(7, 36,  1800.00, 'Combustible',      7, 7, GETDATE(), GETDATE()),  -- detalle 46
(7, 51,  4800.00, 'Alquiler',        7, 7, GETDATE(), GETDATE()),  -- detalle 47
(7, 60,  1500.00, 'Fondo emergencia', 7, 7, GETDATE(), GETDATE());  -- detalle 48
GO
 
-- Sofia (presupuesto 8)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(8, 18, 16000.00, 'Salario',          8, 8, GETDATE(), GETDATE()),  -- detalle 49
(8, 27,  3000.00, 'Supermercado',     8, 8, GETDATE(), GETDATE()),  -- detalle 50
(8, 51,  4000.00, 'Alquiler',        8, 8, GETDATE(), GETDATE()),  -- detalle 51
(8, 60,  1200.00, 'Fondo emergencia', 8, 8, GETDATE(), GETDATE());  -- detalle 52
GO
 
-- Andres (presupuesto 9)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(9, 18, 23000.00, 'Salario',          9, 9, GETDATE(), GETDATE()),  -- detalle 53
(9, 27,  4200.00, 'Supermercado',     9, 9, GETDATE(), GETDATE()),  -- detalle 54
(9, 36,  2200.00, 'Combustible',      9, 9, GETDATE(), GETDATE()),  -- detalle 55
(9, 51,  5500.00, 'Alquiler',        9, 9, GETDATE(), GETDATE()),  -- detalle 56
(9, 57,   700.00, 'Seguro vida',      9, 9, GETDATE(), GETDATE()),  -- detalle 57
(9, 60,  1800.00, 'Fondo emergencia', 9, 9, GETDATE(), GETDATE());  -- detalle 58
GO
 
-- Laura (presupuesto 10)
INSERT INTO prespuesto_detalles (id_presupuesto, id_subcategoria, monto_mensual, observaciones, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(10, 18, 14000.00, 'Salario',          10, 10, GETDATE(), GETDATE()),  -- detalle 59
(10, 27,  2800.00, 'Supermercado',     10, 10, GETDATE(), GETDATE()),  -- detalle 60
(10, 51,  3800.00, 'Alquiler',        10, 10, GETDATE(), GETDATE()),  -- detalle 61
(10, 60,  1000.00, 'Fondo emergencia', 10, 10, GETDATE(), GETDATE());  -- detalle 62
GO

--Obligaciones fijas
INSERT INTO obligaciones_fijas (id_subcategoria, nombre_obligacion, monto_mensual, dia_vencimiento, estado_vigente, fecha_inicio, fecha_fin, creado_por, modificado_por, creado_en, modificado_en)
VALUES
-- Carlos
(51, 'Alquiler Apartamento Carlos',      5000.00,  5, 1, '2025-01-01', NULL, 1, 1, GETDATE(), GETDATE()),  -- oblig 1
(31, 'Energia Electrica Carlos',          800.00, 15, 1, '2025-01-01', NULL, 1, 1, GETDATE(), GETDATE()),  -- oblig 2
(34, 'Internet Carlos',                   700.00, 10, 1, '2025-01-01', NULL, 1, 1, GETDATE(), GETDATE()),  -- oblig 3
(57, 'Seguro de Vida Carlos',             600.00, 20, 1, '2025-01-01', NULL, 1, 1, GETDATE(), GETDATE()),  -- oblig 4
-- Ana
(51, 'Alquiler Apartamento Ana',         4500.00,  5, 1, '2025-01-01', NULL, 2, 2, GETDATE(), GETDATE()),  -- oblig 5
(31, 'Energia Electrica Ana',             700.00, 15, 1, '2025-01-01', NULL, 2, 2, GETDATE(), GETDATE()),  -- oblig 6
(34, 'Internet Ana',                      600.00, 10, 1, '2025-01-01', NULL, 2, 2, GETDATE(), GETDATE()),  -- oblig 7
-- Jose
(51, 'Alquiler Casa Jose',               5500.00,  1, 1, '2025-01-01', NULL, 3, 3, GETDATE(), GETDATE()),  -- oblig 8
(57, 'Seguro Vida Jose',                  700.00, 20, 1, '2025-01-01', NULL, 3, 3, GETDATE(), GETDATE()),  -- oblig 9
(34, 'Internet Jose',                     800.00, 10, 1, '2025-01-01', NULL, 3, 3, GETDATE(), GETDATE()),  -- oblig 10
-- Maria
(51, 'Alquiler Cuarto Maria',            4000.00,  1, 1, '2025-06-01', NULL, 4, 4, GETDATE(), GETDATE()),  -- oblig 11
(31, 'Energia Electrica Maria',           600.00, 15, 1, '2025-06-01', NULL, 4, 4, GETDATE(), GETDATE()),  -- oblig 12
-- Pedro
(51, 'Alquiler Apartamento Pedro',       4500.00,  5, 1, '2025-01-01', NULL, 5, 5, GETDATE(), GETDATE()),  -- oblig 13
(57, 'Seguro Vida Pedro',                 600.00, 25, 1, '2025-01-01', NULL, 5, 5, GETDATE(), GETDATE()),  -- oblig 14
(58, 'Seguro Vehiculo Pedro',            1200.00, 15, 1, '2025-01-01', NULL, 5, 5, GETDATE(), GETDATE());  -- oblig 15
GO

--Transacciones enero 2026 
-- CARLOS enero
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(1,  1, 2026, 1, 18, 'ingreso', 'Salario enero 2026',             25000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t1
(1,  2, 2026, 1, 27, 'gasto',   'Supermercado semana 1',            950.00, '2026-01-05', 'tarjeta_debito', NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t2
(1,  2, 2026, 1, 27, 'gasto',   'Supermercado semana 2',           1100.00, '2026-01-12', 'tarjeta_debito', NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t3
(1,  2, 2026, 1, 27, 'gasto',   'Supermercado semana 3',            980.00, '2026-01-19', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t4
(1,  3, 2026, 1, 28, 'gasto',   'Almuerzo restaurante',             350.00, '2026-01-08', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t5
(1,  3, 2026, 1, 28, 'gasto',   'Cena familiar restaurante',        680.00, '2026-01-22', 'tarjeta_credito',NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t6
(1,  4, 2026, 1, 31, 'gasto',   'Recibo energia enero',             820.00, '2026-01-15', 'transferencia',  1001, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t7
(1,  5, 2026, 1, 32, 'gasto',   'Recibo agua enero',                290.00, '2026-01-15', 'efectivo',       1002, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t8
(1,  6, 2026, 1, 34, 'gasto',   'Internet enero',                   700.00, '2026-01-10', 'transferencia',  1003, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t9
(1,  7, 2026, 1, 36, 'gasto',   'Combustible semana 1',             800.00, '2026-01-03', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t10
(1,  7, 2026, 1, 36, 'gasto',   'Combustible semana 3',             750.00, '2026-01-17', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t11
(1,  8, 2026, 1, 51, 'gasto',   'Alquiler enero',                  5000.00, '2026-01-05', 'transferencia',  NULL, 'Pago puntual', GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t12
(1,  9, 2026, 1, 57, 'gasto',   'Seguro vida enero',                600.00, '2026-01-20', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t13
(1, 10, 2026, 1, 60, 'ahorro',  'Aporte fondo emergencia enero',   2000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t14
(1, 11, 2026, 1, 65, 'ahorro',  'Ahorro vacaciones enero',         2000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE());  -- t15
GO
 
-- ANA enero
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(2, 12, 2026, 1, 18, 'ingreso', 'Salario enero Ana',              18000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t16
(2, 13, 2026, 1, 27, 'gasto',   'Supermercado semana 1 Ana',        800.00, '2026-01-04', 'tarjeta_debito', NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t17
(2, 13, 2026, 1, 27, 'gasto',   'Supermercado semana 3 Ana',        950.00, '2026-01-18', 'tarjeta_debito', NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t18
(2, 14, 2026, 1, 31, 'gasto',   'Recibo energia Ana enero',         680.00, '2026-01-15', 'transferencia',  2001, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t19
(2, 15, 2026, 1, 34, 'gasto',   'Internet Ana enero',               600.00, '2026-01-10', 'transferencia',  2002, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t20
(2, 16, 2026, 1, 51, 'gasto',   'Alquiler Ana enero',              4500.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t21
(2, 17, 2026, 1, 60, 'ahorro',  'Fondo emergencia Ana enero',      1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t22
(2, 18, 2026, 1, 65, 'ahorro',  'Meta vacaciones Ana enero',       1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE());  -- t23
GO
 
-- JOSE enero
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(3, 19, 2026, 1, 18, 'ingreso', 'Salario enero Jose',             22000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t24
(3, 20, 2026, 1, 27, 'gasto',   'Supermercado Jose semana 1',      1100.00, '2026-01-06', 'tarjeta_debito', NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t25
(3, 20, 2026, 1, 27, 'gasto',   'Supermercado Jose semana 2',      1200.00, '2026-01-13', 'tarjeta_debito', NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t26
(3, 21, 2026, 1, 28, 'gasto',   'Restaurante Jose',                 500.00, '2026-01-09', 'efectivo',       NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t27
(3, 22, 2026, 1, 36, 'gasto',   'Combustible Jose',                1200.00, '2026-01-07', 'efectivo',       NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t28
(3, 23, 2026, 1, 51, 'gasto',   'Alquiler Jose enero',             5500.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t29
(3, 24, 2026, 1, 57, 'gasto',   'Seguro vida Jose enero',           700.00, '2026-01-20', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t30
(3, 25, 2026, 1, 60, 'ahorro',  'Fondo emergencia Jose enero',     2000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t31
(3, 26, 2026, 1, 65, 'ahorro',  'Meta vacaciones Jose enero',      1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE());  -- t32
GO
 
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
-- Maria enero
(4, 27, 2026, 1, 18, 'ingreso', 'Salario enero Maria',            15000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t33
(4, 28, 2026, 1, 27, 'gasto',   'Supermercado Maria enero',        2800.00, '2026-01-08', 'tarjeta_debito', NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t34
(4, 29, 2026, 1, 31, 'gasto',   'Energia Maria enero',              580.00, '2026-01-15', 'transferencia',  3001, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t35
(4, 30, 2026, 1, 51, 'gasto',   'Alquiler Maria enero',            4000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t36
(4, 31, 2026, 1, 60, 'ahorro',  'Fondo emergencia Maria enero',    1000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t37
(4, 32, 2026, 1, 65, 'ahorro',  'Meta vacaciones Maria enero',     1000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t38
-- Pedro enero
(5, 33, 2026, 1, 18, 'ingreso', 'Salario enero Pedro',            20000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t39
(5, 34, 2026, 1, 27, 'gasto',   'Supermercado Pedro enero',        3800.00, '2026-01-07', 'tarjeta_debito', NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t40
(5, 35, 2026, 1, 36, 'gasto',   'Combustible Pedro enero',         1900.00, '2026-01-10', 'efectivo',       NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t41
(5, 36, 2026, 1, 51, 'gasto',   'Alquiler Pedro enero',            4500.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t42
(5, 37, 2026, 1, 57, 'gasto',   'Seguro vida Pedro enero',          600.00, '2026-01-25', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t43
(5, 38, 2026, 1, 60, 'ahorro',  'Fondo emergencia Pedro enero',    1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t44
(5, 39, 2026, 1, 65, 'ahorro',  'Meta vacaciones Pedro enero',     1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t45
-- Lucia enero
(6, 40, 2026, 1, 18, 'ingreso', 'Salario enero Lucia',            17500.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t46
(6, 41, 2026, 1, 27, 'gasto',   'Supermercado Lucia enero',        3100.00, '2026-01-09', 'tarjeta_debito', NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t47
(6, 42, 2026, 1, 51, 'gasto',   'Alquiler Lucia enero',            4200.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t48
(6, 43, 2026, 1, 60, 'ahorro',  'Fondo emergencia Lucia enero',    1300.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t49
-- Diego enero
(7, 44, 2026, 1, 18, 'ingreso', 'Salario enero Diego',            19000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t50
(7, 45, 2026, 1, 27, 'gasto',   'Supermercado Diego enero',        3600.00, '2026-01-06', 'tarjeta_debito', NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t51
(7, 46, 2026, 1, 36, 'gasto',   'Combustible Diego enero',         1700.00, '2026-01-08', 'efectivo',       NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t52
(7, 47, 2026, 1, 51, 'gasto',   'Alquiler Diego enero',            4800.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t53
(7, 48, 2026, 1, 60, 'ahorro',  'Fondo emergencia Diego enero',    1500.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t54
-- Sofia enero
(8, 49, 2026, 1, 18, 'ingreso', 'Salario enero Sofia',            16000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t55
(8, 50, 2026, 1, 27, 'gasto',   'Supermercado Sofia enero',        2900.00, '2026-01-07', 'tarjeta_debito', NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t56
(8, 51, 2026, 1, 51, 'gasto',   'Alquiler Sofia enero',            4000.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t57
(8, 52, 2026, 1, 60, 'ahorro',  'Fondo emergencia Sofia enero',    1200.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t58
-- Andres enero
(9, 53, 2026, 1, 18, 'ingreso', 'Salario enero Andres',           23000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t59
(9, 54, 2026, 1, 27, 'gasto',   'Supermercado Andres enero',       4000.00, '2026-01-05', 'tarjeta_debito', NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t60
(9, 55, 2026, 1, 36, 'gasto',   'Combustible Andres enero',        2100.00, '2026-01-09', 'efectivo',       NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t61
(9, 56, 2026, 1, 51, 'gasto',   'Alquiler Andres enero',           5500.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t62
(9, 57, 2026, 1, 57, 'gasto',   'Seguro vida Andres enero',         700.00, '2026-01-20', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t63
(9, 58, 2026, 1, 60, 'ahorro',  'Fondo emergencia Andres enero',   1800.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t64
-- Laura enero
(10, 59, 2026, 1, 18, 'ingreso','Salario enero Laura',            14000.00, '2026-01-01', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t65
(10, 60, 2026, 1, 27, 'gasto',  'Supermercado Laura enero',        2700.00, '2026-01-08', 'tarjeta_debito', NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t66
(10, 61, 2026, 1, 51, 'gasto',  'Alquiler Laura enero',            3800.00, '2026-01-05', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t67
(10, 62, 2026, 1, 60, 'ahorro', 'Fondo emergencia Laura enero',    1000.00, '2026-01-31', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE());  -- t68
GO
 
--febrero 2026
-- CARLOS febrero
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
(1,  1, 2026, 2, 18, 'ingreso', 'Salario febrero 2026',           25000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t69
(1,  2, 2026, 2, 27, 'gasto',   'Supermercado semana 1 feb',       1050.00, '2026-02-02', 'tarjeta_debito', NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t70
(1,  2, 2026, 2, 27, 'gasto',   'Supermercado semana 2 feb',       1150.00, '2026-02-09', 'tarjeta_debito', NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t71
(1,  2, 2026, 2, 27, 'gasto',   'Supermercado semana 3 feb',        900.00, '2026-02-16', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t72
(1,  3, 2026, 2, 28, 'gasto',   'Cena San Valentin restaurante',    950.00, '2026-02-14', 'tarjeta_credito',NULL, 'San Valentin', GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t73
(1,  4, 2026, 2, 31, 'gasto',   'Recibo energia febrero',           780.00, '2026-02-15', 'transferencia',  1004, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t74
(1,  5, 2026, 2, 32, 'gasto',   'Recibo agua febrero',              310.00, '2026-02-15', 'efectivo',       1005, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t75
(1,  6, 2026, 2, 34, 'gasto',   'Internet febrero',                 700.00, '2026-02-10', 'transferencia',  1006, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t76
(1,  7, 2026, 2, 36, 'gasto',   'Combustible febrero semana 1',     850.00, '2026-02-04', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t77
(1,  7, 2026, 2, 36, 'gasto',   'Combustible febrero semana 3',     700.00, '2026-02-18', 'efectivo',       NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t78
(1,  8, 2026, 2, 51, 'gasto',   'Alquiler febrero',                5000.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t79
(1,  9, 2026, 2, 57, 'gasto',   'Seguro vida febrero',              600.00, '2026-02-20', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t80
(1, 10, 2026, 2, 60, 'ahorro',  'Fondo emergencia febrero',        2000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE()),  -- t81
(1, 11, 2026, 2, 65, 'ahorro',  'Ahorro vacaciones febrero',       2000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 1, 1, GETDATE(), GETDATE());  -- t82
GO
INSERT INTO transacciones (id_usuario, id_detalle, anio_transaccion, mes_transaccion, id_subcategoria, tipo_transaccion, descripcion_movimiento, monto_transaccion, fecha_transaccion, metodo_pago, numero_factura, observaciones, fecha_y_hora_registro, creado_por, modificado_por, creado_en, modificado_en)
VALUES
-- Ana febrero
(2, 12, 2026, 2, 18, 'ingreso', 'Salario febrero Ana',            18000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t83
(2, 13, 2026, 2, 27, 'gasto',   'Supermercado Ana feb semana 1',     820.00, '2026-02-03', 'tarjeta_debito', NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t84
(2, 13, 2026, 2, 27, 'gasto',   'Supermercado Ana feb semana 3',    1020.00, '2026-02-17', 'tarjeta_debito', NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t85
(2, 14, 2026, 2, 31, 'gasto',   'Energia Ana febrero',               710.00, '2026-02-15', 'transferencia',  2003, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t86
(2, 15, 2026, 2, 34, 'gasto',   'Internet Ana febrero',              600.00, '2026-02-10', 'transferencia',  2004, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t87
(2, 16, 2026, 2, 51, 'gasto',   'Alquiler Ana febrero',             4500.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t88
(2, 17, 2026, 2, 60, 'ahorro',  'Fondo emergencia Ana feb',         1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t89
(2, 18, 2026, 2, 65, 'ahorro',  'Meta vacaciones Ana feb',          1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 2, 2, GETDATE(), GETDATE()),  -- t90
-- Jose febrero
(3, 19, 2026, 2, 18, 'ingreso', 'Salario febrero Jose',            22000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t91
(3, 20, 2026, 2, 27, 'gasto',   'Supermercado Jose feb',            2500.00, '2026-02-06', 'tarjeta_debito', NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t92
(3, 21, 2026, 2, 28, 'gasto',   'San Valentin restaurante Jose',     750.00, '2026-02-14', 'tarjeta_credito',NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t93
(3, 22, 2026, 2, 36, 'gasto',   'Combustible Jose feb',             1350.00, '2026-02-08', 'efectivo',       NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t94
(3, 23, 2026, 2, 51, 'gasto',   'Alquiler Jose febrero',            5500.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t95
(3, 24, 2026, 2, 57, 'gasto',   'Seguro vida Jose febrero',          700.00, '2026-02-20', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t96
(3, 25, 2026, 2, 60, 'ahorro',  'Fondo emergencia Jose feb',        2000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t97
(3, 26, 2026, 2, 65, 'ahorro',  'Meta vacaciones Jose feb',         1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 3, 3, GETDATE(), GETDATE()),  -- t98
-- Maria febrero
(4, 27, 2026, 2, 18, 'ingreso', 'Salario febrero Maria',           15000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t99
(4, 28, 2026, 2, 27, 'gasto',   'Supermercado Maria feb',           3100.00, '2026-02-09', 'tarjeta_debito', NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t100
(4, 29, 2026, 2, 31, 'gasto',   'Energia Maria febrero',             610.00, '2026-02-15', 'transferencia',  3002, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t101
(4, 30, 2026, 2, 51, 'gasto',   'Alquiler Maria febrero',           4000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t102
(4, 31, 2026, 2, 60, 'ahorro',  'Fondo emergencia Maria feb',       1000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t103
(4, 32, 2026, 2, 65, 'ahorro',  'Meta vacaciones Maria feb',        1000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 4, 4, GETDATE(), GETDATE()),  -- t104
-- Pedro febrero
(5, 33, 2026, 2, 18, 'ingreso', 'Salario febrero Pedro',           20000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t105
(5, 34, 2026, 2, 27, 'gasto',   'Supermercado Pedro feb',           4100.00, '2026-02-07', 'tarjeta_debito', NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t106
(5, 35, 2026, 2, 36, 'gasto',   'Combustible Pedro feb',            2050.00, '2026-02-11', 'efectivo',       NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t107
(5, 36, 2026, 2, 51, 'gasto',   'Alquiler Pedro febrero',           4500.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t108
(5, 37, 2026, 2, 57, 'gasto',   'Seguro vida Pedro feb',             600.00, '2026-02-25', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t109
(5, 38, 2026, 2, 60, 'ahorro',  'Fondo emergencia Pedro feb',       1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t110
(5, 39, 2026, 2, 65, 'ahorro',  'Meta vacaciones Pedro feb',        1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 5, 5, GETDATE(), GETDATE()),  -- t111
-- Lucia febrero
(6, 40, 2026, 2, 18, 'ingreso', 'Salario febrero Lucia',           17500.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t112
(6, 41, 2026, 2, 27, 'gasto',   'Supermercado Lucia feb',           3300.00, '2026-02-10', 'tarjeta_debito', NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t113
(6, 42, 2026, 2, 51, 'gasto',   'Alquiler Lucia febrero',           4200.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t114
(6, 43, 2026, 2, 60, 'ahorro',  'Fondo emergencia Lucia feb',       1300.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 6, 6, GETDATE(), GETDATE()),  -- t115
-- Diego febrero
(7, 44, 2026, 2, 18, 'ingreso', 'Salario febrero Diego',           19000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t116
(7, 45, 2026, 2, 27, 'gasto',   'Supermercado Diego feb',           3800.00, '2026-02-07', 'tarjeta_debito', NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t117
(7, 46, 2026, 2, 36, 'gasto',   'Combustible Diego feb',            1850.00, '2026-02-09', 'efectivo',       NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t118
(7, 47, 2026, 2, 51, 'gasto',   'Alquiler Diego febrero',           4800.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t119
(7, 48, 2026, 2, 60, 'ahorro',  'Fondo emergencia Diego feb',       1500.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 7, 7, GETDATE(), GETDATE()),  -- t120
-- Sofia febrero
(8, 49, 2026, 2, 18, 'ingreso', 'Salario febrero Sofia',           16000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t121
(8, 50, 2026, 2, 27, 'gasto',   'Supermercado Sofia feb',           3050.00, '2026-02-08', 'tarjeta_debito', NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t122
(8, 51, 2026, 2, 51, 'gasto',   'Alquiler Sofia febrero',           4000.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t123
(8, 52, 2026, 2, 60, 'ahorro',  'Fondo emergencia Sofia feb',       1200.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 8, 8, GETDATE(), GETDATE()),  -- t124
-- Andres febrero
(9, 53, 2026, 2, 18, 'ingreso', 'Salario febrero Andres',          23000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t125
(9, 54, 2026, 2, 27, 'gasto',   'Supermercado Andres feb',          4300.00, '2026-02-06', 'tarjeta_debito', NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t126
(9, 55, 2026, 2, 36, 'gasto',   'Combustible Andres feb',           2200.00, '2026-02-10', 'efectivo',       NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t127
(9, 56, 2026, 2, 51, 'gasto',   'Alquiler Andres febrero',          5500.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t128
(9, 57, 2026, 2, 57, 'gasto',   'Seguro vida Andres feb',            700.00, '2026-02-20', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t129
(9, 58, 2026, 2, 60, 'ahorro',  'Fondo emergencia Andres feb',      1800.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 9, 9, GETDATE(), GETDATE()),  -- t130
-- Laura febrero
(10, 59, 2026, 2, 18, 'ingreso','Salario febrero Laura',           14000.00, '2026-02-01', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t131
(10, 60, 2026, 2, 27, 'gasto',  'Supermercado Laura feb',           2850.00, '2026-02-09', 'tarjeta_debito', NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t132
(10, 61, 2026, 2, 51, 'gasto',  'Alquiler Laura febrero',           3800.00, '2026-02-05', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE()),  -- t133
(10, 62, 2026, 2, 60, 'ahorro', 'Fondo emergencia Laura feb',       1000.00, '2026-02-28', 'transferencia',  NULL, NULL, GETDATE(), 10,10, GETDATE(), GETDATE());  -- t134
GO

--transacciones obligaciones fijas
--1=Alquiler Carlos, 2=Energia Carlos
--3=Internet Carlos,  4=Seguro Carlos
--5=Alquiler Ana,     6=Energia Ana, 7=Internet Ana
--8=Alquiler Jose,    9=Seguro Jose, 10=Internet Jose
--11=Alquiler Maria, 12=Energia Maria
--13=Alquiler Pedro, 14=Seguro Pedro
INSERT INTO transacciones_obligaciones_fijas (id_transaccion, id_obligacion)
VALUES
-- Carlos enero: alquiler(t12), energia(t7), internet(t9), seguro(t13)
(12, 1), (7, 2), (9, 3), (13, 4),
-- Carlos febrero: alquiler(t79), energia(t74), internet(t76), seguro(t80)
(79, 1), (74, 2), (76, 3), (80, 4),
-- Ana enero: alquiler(t21), energia(t19), internet(t20)
(21, 5), (19, 6), (20, 7),
-- Ana febrero: alquiler(t88), energia(t86), internet(t87)
(88, 5), (86, 6), (87, 7),
-- Jose enero: alquiler(t29), seguro(t30)
(29, 8), (30, 9),
-- Jose febrero: alquiler(t95), seguro(t96)
(95, 8), (96, 9),
-- Maria enero: alquiler(t36), energia(t35)
(36, 11), (35, 12),
-- Maria febrero: alquiler(t102), energia(t101)
(102, 11), (101, 12),
-- Pedro enero: alquiler(t42), seguro vida(t43)
(42, 13), (43, 14),
-- Pedro febrero: alquiler(t108), seguro vida(t109)
(108, 13), (109, 14);
GO