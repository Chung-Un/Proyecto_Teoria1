# Gestor de Presupuestos Personales
> **Teoría de Base de Datos I**

Una solución completa para la gestión financiera personal que facilita la planificación de presupuestos, el seguimiento de transacciones, el control de obligaciones fijas y el cumplimiento de metas de ahorro.

---

## Descripción General

El **Gestor de Presupuestos Personales** es una aplicación de consola desarrollada en C# con .NET 10 que permite a los usuarios tomar control de sus finanzas personales. Permite registrar ingresos, gastos y ahorros, crear presupuestos mensuales detallados, gestionar obligaciones fijas recurrentes y visualizar el progreso financiero mediante reportes PDF con gráficos.

---

## Objetivos

Aplicar los conocimientos de **Teoría de Base de Datos I** mediante el diseño, implementación y despliegue de una solución completa de gestión financiera personal utilizando SQL Server, stored procedures, funciones, triggers y una aplicación de consola en C#.

---

## Características Principales

| Módulo | Descripción |
|--------|-------------|
| **Gestión de Usuarios** | Registro, autenticación y administración de perfiles. Solo el administrador puede gestionar usuarios |
| **Categorías y Subcategorías** | Organización de ingresos, gastos y ahorros por categorías y subcategorías personalizadas |
| **Planificación Presupuestal** | Creación de presupuestos con desglose por subcategoría y montos mensuales asignados |
| **Registro de Transacciones** | Control detallado de ingresos, gastos y ahorros vinculados a presupuestos activos |
| **Obligaciones Fijas** | Administración de pagos recurrentes con seguimiento de fechas de vencimiento y estados de pago |
| **Reportes PDF** | Generación de 6 reportes con gráficos usando QuestPDF y ScottPlot |

---

## Tecnologías Utilizadas

- **Microsoft SQL Server Express** — Motor de base de datos relacional
- **C# con .NET 10** — Lenguaje y plataforma de desarrollo
- **ADO.NET (Microsoft.Data.SqlClient)** — Conexión entre C# y SQL Server
- **QuestPDF** — Generación de documentos PDF
- **ScottPlot** — Generación de gráficos (barras, pie, líneas)

---

## Credenciales de Administrador

| Rol | Correo | Contraseña |
|-----|--------|------------|
| Administrador | admin@presupuesto.hn | ADMIN@2026! |

---

## Reportes Disponibles

| # | Nombre | Tipo de Gráfico |
|---|--------|----------------|
| 1 | Resumen Mensual de Ingresos vs Gastos vs Ahorros | Barras agrupadas |
| 2 | Distribución de Gastos por Categoría | Pie/Dona |
| 3 | Análisis de Cumplimiento de Presupuesto | Barras comparativas |
| 4 | Tendencia de Gastos por Categoría en el Tiempo | Líneas múltiples |
| 5 | Estado de Obligaciones Fijas | Tabla + Pie |
| 6 | Progreso de Metas de Ahorro | Barras comparativas |

Los PDFs se generan en la carpeta `Reportes/` dentro del directorio de ejecución.

---

## Estructura del Proyecto
```
Proyecto_Teoria1/
├── README.md
│
├── 📁 database/
│   ├── DDL/
│   │   └── 01_crear_tablas.sql
│   ├── procedimientos/
│   │   ├── CRUD_USUARIO.sql
│   │   ├── CRUD_CATEGORIA.sql
│   │   ├── CRUD_SUBCATEGORIA.sql
│   │   ├── CRUD_PRESUPUESTO.sql
│   │   ├── CRUD_PRESUPUESTO_DETALLE.sql
│   │   ├── CRUD_OBLIGACION_FIJA.sql
│   │   ├── CRUD_TRANSACCION.sql
│   │   └── LOGICANEGOCIOS.sql
│   ├── funciones/
│   │   └── FUNCIONES.sql
│   ├── triggers/
│   │   └── TRIGGERS.sql
│   └── datos_prueba/
│       └── INSERT_DATOS.sql
│
└── 📁 PresupuestoPersonal/
    ├── Program.cs
    ├── 📁 Models/
    │   ├── Usuario.cs
    │   ├── Categoria.cs
    │   ├── Subcategoria.cs
    │   ├── Presupuesto.cs
    │   ├── PresupuestoDetalle.cs
    │   ├── ObligacionFija.cs
    │   └── Transaccion.cs
    ├── 📁 DataAccess/
    │   ├── Conexion.cs
    │   ├── UsuarioDAL.cs
    │   ├── CategoriaDAL.cs
    │   ├── SubcategoriaDAL.cs
    │   ├── PresupuestoDAL.cs
    │   ├── PresupuestoDetalleDAL.cs
    │   ├── ObligacionFijaDAL.cs
    │   └── TransaccionDAL.cs
    ├── 📁 Validaciones/
    │   ├── UsuarioValidaciones.cs
    │   ├── CategoriaValidaciones.cs
    │   ├── SubcategoriaValidaciones.cs
    │   ├── PresupuestoValidaciones.cs
    │   ├── PresupuestoDetalleValidaciones.cs
    │   ├── ObligacionFijaValidaciones.cs
    │   └── TransaccionValidaciones.cs
    ├── 📁 Menus/
    │   ├── MenuPrincipal.cs
    │   ├── MenuUsuarios.cs
    │   ├── MenuCategorias.cs
    │   ├── MenuSubcategorias.cs
    │   ├── MenuPresupuestos.cs
    │   ├── MenuPresupuestoDetalles.cs
    │   ├── MenuTransacciones.cs
    │   ├── MenuObligaciones.cs
    │   └── MenuReportes.cs
    └── 📁 Reportes/
        ├── ReporteResumenMensual.cs
        ├── ReporteDistribucionGastos.cs
        ├── ReporteCumplimientoPresupuesto.cs
        ├── ReporteTendenciaGastos.cs
        ├── ReporteObligacionesFijas.cs
        └── ReporteProgresoAhorros.cs
```

---

## Arquitectura

El proyecto sigue una arquitectura de 3 capas:
```
MENUS (Presentación)
      ↓
VALIDACIONES (Lógica de negocio)
      ↓
DAL - Data Access Layer (Acceso a datos)
      ↓
SQL SERVER (Base de datos)
```

- **Menus** — Interacción con el usuario, muestra pantallas y recibe inputs
- **Validaciones** — Verifica que los datos sean correctos antes de guardarlos
- **DAL** — Ejecuta los stored procedures y mapea los resultados a modelos
- **Models** — Clases que representan las entidades del sistema

---

## Base de Datos

### Triggers destacados

- **tgr_crear_subcategoria_defecto** — Crea automáticamente la subcategoría `General` al insertar una categoría nueva
- **trg_validar_vigencia_transaccion** — Valida que la fecha de una transacción esté dentro del rango del presupuesto

---

## Requisitos para Ejecutar

1. **SQL Server Express** instalado y corriendo en `localhost\SQLEXPRESS`
2. **.NET 10 SDK** instalado
3. Ejecutar los scripts en este orden:
   - `01_crear_tablas.sql`
   - Todos los archivos de `procedimientos/`
   - `FUNCIONES.sql`
   - `TRIGGERS.sql`
   - `INSERT_DATOS.sql`
4. Compilar y ejecutar el proyecto `PresupuestoPersonal`

---

## Configuración de Conexión

La cadena de conexión está en `DataAccess/Conexion.cs`:
```
Server=localhost\SQLEXPRESS;Database=PresupuestoPersonal;Integrated Security=True;TrustServerCertificate=True
```

Usa **Windows Authentication** — no requiere usuario ni contraseña adicional.

# Autoevaluación

**Estudiante: Chung Un Yum**  
**Fecha: 24/03/2026**  

---

## 1. Reflexión sobre el Proceso de Desarrollo

> *La realización de este proyecto me ha ayudado a reforzar los conocimientos adquiridos en la clase de Teoría de Base de Datos 1. Desde la creacion de tablas, hasta la conexion entre una base de datos y un programa, todo fue un gran proceso de aprendizaje.*

### 1.1 Diseño de la Base de Datos
*Las relaciones entre entidades fueron revisadas en clase, esto se evidencia con la imagen adjunta en el folder de documentos.*

### 1.2 Desarrollo del Código
*El codigo se fue realizando en etapas, primero la conexion, luego la definicion de las entidades, los procedimientos, menus y reportes.*

### 1.3 Pruebas
*Se realizaron pruebas con ambos el usuario administrador y usuarios normales.*
---

## 2. Desafíos Enfrentados 

> *Los principales desafios fueron los reportes, especialmente los procedimientos atados a estos debido al uso de crosstab.*
> *Partes en las que se utilizó ayuda de IA: el frontend, los reportes y la generacion de datos.*

## 3. Aprendizajes Clave

> *Como se mencionó al incio, la realización del proyecto ayudó a reforzar los conocimmientos adquiridos en clase: procedimientos, triggers, crosstabs, conexiones, etc.*

### Aprendizaje Personal más Valioso
>*El proyecto me enseño la importancia de las bases de datos, en clases anteriores, realizar un proyecto como este sin base de datos hubiera resultado mucho mas tedioso y fastidioso.*

---

## 4. Sugerencias de Mejora del Proyecto

> *Mejoraria la parte visual del proyecto, la haría mas agradable para el usuario; además, tendria un sistema de menus mas complejos y completos.*



---

