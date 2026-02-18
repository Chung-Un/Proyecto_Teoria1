## Gestor de Presupuestos Personales
> **Teoría de Base de Datos I**

Una solución completa para la gestión financiera personal que facilita la planificación de tus finanzas, el seguimiento de obligaciones y el cumplimiento de metas de ahorro.

---

## Descripción General

El **Gestor de Presupuestos Personales** es una aplicación diseñada para ayudar a los usuarios a tomar control de sus finanzas. Permite registrar ingresos, gastos, obligaciones fijas mensuales y metas de ahorro, brindando una visión clara y organizada del estado financiero personal.

---

## Objetivos

Aplicar los conocimientos de **Teoria de Base de Datos I** mediante el diseño, implementación y despliegue de una solución completa de gestión financiera personal.

---

## Características Principales

| Módulo | Descripción |
|--------|-------------|
| **Gestión de Usuarios** | Registro, autenticación y administración de perfiles de usuario |
| **Planificación Presupuestal** | Creación y seguimiento de presupuestos mensuales personalizados |
| **Registro de Transacciones** | Control detallado de ingresos y egresos |
| **Gestión de Obligaciones Fijas** | Administración de pagos recurrentes y compromisos mensuales |
| **Analítica y Reportería** | Visualización de datos y generación de reportes financieros |

---

## Tecnologías Utilizadas

- **Microsoft SQL Server** — Motor de base de datos relacional
- **C#** — Lenguaje de programación principal
- **Entity Framework (LINQ)** — ORM para el acceso a datos
- **Frontend** — Interfaz de usuario simplística

---

## Estructura del Proyecto
```
proyecto-presupuesto-personal/
├── README.md                        # Descripción del proyecto
│
├── 📁 docs/
│   ├── ModeloRelacional.pdf         # Modelo Relacional documentado
│   ├── ArchivoSQL.sql               # Diccionario de datos
│   └── Reportes.pdf                 # Documentación de reportes con SQL
│
├── 📁 database/
│   ├── DDL/
│   │   └── 01_crear_tablas.sql
│   ├── procedimientos/
│   │   ├── crud_usuario.sql
│   │   ├── crud_categoria.sql
│   │   └── ... (otros procedimientos)
│   ├── funciones/
│   │   └── funciones.sql
│   ├── triggers/
│   │   └── triggers.sql
│   └── datos_prueba/
│       └── insertar_datos.sql
│
├── 📁 backend/
│   ├── src/
│   ├── package.json (o equivalente)
│   └── README.md
│
├── 📁 frontend/
│   ├── src/
│   ├── assets/
│   └── README.md
│
└── 📁 metabase/
    └── metabase_backup.zip
```
