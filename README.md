# API REST para la Gestión de Gastos Personales
## Entidades y Atributos
### 1. Usuarios
- id (UUID): Identificador único del usuario.
- nombre (string): Nombre completo del usuario.
- email (string): Correo electrónico del usuario (debe ser único).
- password (string): Contraseña del usuario (encriptada).
- fecha_creacion (timestamp): Fecha y hora de creación del usuario.
- fecha_actualizacion (timestamp): Fecha y hora de la última actualización de los datos del usuario.
### 2. Categorías de Gastos (personalizadas por usuario)
- id (UUID): Identificador único de la categoría.
- nombre (string): Nombre de la categoría (ej., Alimentos, Transporte).
- descripcion (string): Descripción de la categoría.
- usuario_id (UUID): Identificador del usuario que creó la categoría.
### 3. Gastos
- id (UUID): Identificador único del gasto.
- monto (decimal): Monto del gasto.
- fecha (date): Fecha en que se realizó el gasto.
- descripcion (string): Descripción del gasto.
- categoria_id (UUID): Identificador de la categoría asociada al gasto.
- usuario_id (UUID): Identificador del usuario que registró el gasto.

## CRUDs de las Entidades
### Usuarios
- Creación: Permite crear un nuevo usuario.
- Cambio de contraseña: Permite cambiar la contraseña del usuario.
- Actualización: Permite actualizar datos del usuario, incluyendo opciones de Soft Delete y Bloqueo.
- Login: Permite al usuario iniciar sesión, devolviendo un token de autenticación.

## Categorías de Gastos
- Creación, Lectura, Actualización y Eliminación (CRUD): Permite gestionar las categorías de gastos de un usuario.
Gastos
- Creación, Lectura, Actualización y Eliminación (CRUD): Permite gestionar los gastos de un usuario.

### Listar según filtros:
- Página: Número de página (ej. ?pagina=1).
- Tamaño de la página: Cantidad de elementos por página (ej. ?tamano_pagina=10).
- Palabra: Texto a buscar en la descripción (sin distinción de mayúsculas ni acentos).
- Categoría: Filtrar por ID de categoría.
Ordenamiento:
- Monto: Ordenar los resultados por monto, de forma ascendente o descendente.
- Fecha: Ordenar los resultados por fecha, de forma ascendente o descendente.

## Paginación
- El listado de gastos debe ser paginado.
- Número de página: Indica la página actual.
- Tamaño de página: Cantidad de ítems por página.
- Total de páginas: Calculado con base en el total de elementos y el tamaño de página.

### Validaciones:
- No permite valores negativos en los parámetros.
- Ignora mayúsculas y acentos en la búsqueda.