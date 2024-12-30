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
- [x] Creación: Permite crear un nuevo usuario.
- [x] Cambio de contraseña: Permite cambiar la contraseña del usuario.
- [x] Actualización: Permite actualizar datos del usuario, incluyendo opciones de Soft Delete y Bloqueo.
- [x] Login: Permite al usuario iniciar sesión, devolviendo un token de autenticación.

## Categorías de Gastos
- [x] Creación, Lectura, Actualización y Eliminación (CRUD): Permite gestionar las categorías de gastos de un usuario.

### Gastos
- [x] Creación, Lectura, Actualización y Eliminación (CRUD): Permite gestionar los gastos de un usuario.

### Listar según filtros:
- [x] Página: Número de página (ej. ?pagina=1).
- [x] Tamaño de la página: Cantidad de elementos por página (ej. ?tamano_pagina=10).
- [x] Palabra: Texto a buscar en la descripción (sin distinción de mayúsculas ni acentos).
- [x] Categoría: Filtrar por ID de categoría.
Ordenamiento:
- [x] Monto: Ordenar los resultados por monto, de forma ascendente o descendente.
- [x] Fecha: Ordenar los resultados por fecha, de forma ascendente o descendente.

## Paginación
- [x] El listado de gastos debe ser paginado.
- [x] Número de página: Indica la página actual.
- [x] Tamaño de página: Cantidad de ítems por página.
- [x] Total de páginas: Calculado con base en el total de elementos y el tamaño de página.

### Validaciones:
- [x] No permite valores negativos en los parámetros.
- [x] Ignora mayúsculas y acentos en la búsqueda.

## Endpoints Extras
- [x] Generar un Qr pasando el {id} de un gasto.
- [x] El Qr tendrá información como:
    - El nombre del usuario al que esta relacionado el gasto.
    - Monto del gasto.
    - La fecha del gasto.
    - La descripción del gasto.
- [x] Obtener todos los usuarios utilizando páginado.
- [x] Generar un reporte en un pdf de los gastos.
    - Se deberá proporcionar un rango para generar el reporte
- [x] Generar un gráfico de barras segun la Categoría de Gastos

### Paginación de usuarios:
- [x] Número de página.
- [x] Tamaño de la página.
- [x] Ordenar por los campos de {IsDeleted} y {IsBlocked}

# Endpoints

### Category Expense

`POST` `/create-category-expense`
```json
{
  "name": "string",
  "description": "string",
  "userId": 0
}
```

`GET` `get-category-expense/{id}`

`Response Body`
```json
{
  "id": 1,
  "name": "Transporte",
  "description": "Gastos de transporte como gasolina, transporte público, etc..",
  "userId": 1,
  "userName": "Jonh Doe"
}
```

`PUT` `/update-category-expense/{id}`
```json
{
  "name": "string",
  "description": "string",
  "userId": 0
}
```

`Response Body`
```json
{
  "id": 1,
  "name": "Transporte",
  "description": "Gastos de transporte como gasolina, transporte público",
  "userId": 1,
  "userName": "Jonh Doe"
}
```

`DELETE` `/delete-category-expense/{id}`

`Response Body`
```json
{
  "id": 1,
  "name": "Transporte",
  "description": "Gastos de transporte como gasolina, transporte público",
  "userId": 1,
  "userName": "Jonh Doe"
}
```

`GET` `/get-all-category-expense`

`Response Body`
```json
[
  {
    "id": 3,
    "name": "Entretenimiento",
    "description": "Gastos de actividades recreativas como cine, conciertos, etc.",
    "userId": 1,
    "userName": "Brian Chaparro"
  },
  {
    "id": 4,
    "name": "Vivienda",
    "description": "Alquiler, hipoteca, servicios de agua, luz, gas, etc.",
    "userId": 2,
    "userName": "Griselda SantaCruz"
  },
  {
    "id": 9,
    "name": "Transporte",
    "description": "Gasolina, mantenimiento de vehículos, transporte público, taxis, etc.",
    "userId": 1,
    "userName": "Brian Chaparro"
  }
]
```
---

### Expense
`POST` `/create-expense`
```json
{
  "amount": 0,
  "date": "2024-12-26T17:11:55.966Z",
  "description": "string",
  "userId": 0,
  "expenseCategoryId": 0
}
```

`GET` `/get-expense/{id}`

`Response Body`
```json
{
  "id": 520,
  "userId": 19,
  "categoryExpenseId": 3,
  "amount": 18.54,
  "date": "6/5/2024",
  "description": "Other intervertebral disc displacement, lumbar region",
  "userName": "Mauricio",
  "isDeleted": false,
  "isBlocked": false,
  "expenseCategoryName": "Entretenimiento"
}
```

`GET` `/generate-expense-qr/{id}`

`Response Body`

`Generate a QR Image`
![qrimage](./img/qr.png)

`GET` `/get-all-expenses`

`Response Body`

`Use a paginate, by default PageSize is 10, you can filter by a description an by a Category Expense Id`
```json
[
  {
    "id": 520,
    "userId": 19,
    "categoryExpenseId": 3,
    "amount": 18.54,
    "date": "6/5/2024",
    "description": "Other intervertebral disc displacement, lumbar region",
    "userName": "Mauricio",
    "isDeleted": false,
    "isBlocked": false,
    "expenseCategoryName": "Entretenimiento"
  },
  {
    "id": 1451,
    "userId": 1,
    "categoryExpenseId": 3,
    "amount": 40.49,
    "date": "3/1/2024",
    "description": "Therapeutic and rehab radiological devices assoc w incdt",
    "userName": "Brian Chaparro",
    "isDeleted": true,
    "isBlocked": false,
    "expenseCategoryName": "Entretenimiento"
  }
]
```

`PUT` `/update-expense/{id}`
```json
{
  "amount": 0,
  "date": "2024-12-26T17:19:38.244Z",
  "description": "string",
  "userId": 0,
  "expenseCategoryId": 0
}


```

`Response Body`

```json
{
  "id": 520,
  "userId": 19,
  "categoryExpenseId": 3,
  "amount": 18,
  "date": "26/12/2024",
  "description": "Other intervertebral disc displacement",
  "userName": "Mauricio",
  "isDeleted": false,
  "isBlocked": false,
  "expenseCategoryName": "Entretenimiento"
}
```

`DELETE` `/delete-expense/{id}`

`Response Body`

```json
{
  "id": 520,
  "userId": 19,
  "categoryExpenseId": 3,
  "amount": 18,
  "date": "26/12/2024",
  "description": "Other intervertebral disc displacement",
  "userName": "Mauricio",
  "isDeleted": false,
  "isBlocked": false,
  "expenseCategoryName": "Entretenimiento"
}
```
`GET` `/generate-chart-report`

`Response Body`

`Generate a Chart Report in pdf format and you can asing a custom name with the {fileName} parameter`


`GET` `/get-pdf-expense-report/{range}`

`Response Body`

`The Endpoint will return a pdf file with the expense report for the given range`

```sh
Download file
```

---

### User
`POST` `/create-user`

```json
{
  "name": "string",
  "email": "string",
  "password": "string"
}
```

`Response Body`
```json
{
  "userId": 521,
  "name": "test2",
  "email": "test2@gmail.com",
  "isDeleted": false,
  "isBlocked": false,
  "created_At": "26/12/2024",
  "updated_At": "26/12/2024"
}
```

`PUT` `/update-user-password`

`[FromQuery]` `email`
```json
{
  "verifyPassword": "string",
  "newPassword": "string"
}
```

`Response Body`

`Contraseña cambiada con éxito`

`PUT` `/update-user`

`[FromQuery]` `email`
```json
{
  "name": "string",
  "email": "string",
  "isDeleted": true,
  "isBlocked": true
}
```

`Response Body`

```json
{
  "userId": 521,
  "name": "test2user",
  "email": "test2user@gmail.com",
  "isDeleted": true,
  "isBlocked": true,
  "created_At": "26/12/2024",
  "updated_At": "26/12/2024"
}
```

`POST` `/login`

```json
{
  "email": "string",
  "password": "string"
}
```

`Response Body`

```cs
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI1MjEiLCJuYW1lIjoidGVzdDJ1c2VyIiwianRpIjoiZmEwM2I2NzQtNmIxNS00M2U5LWIzZmUtODAwYTQyNDI4YzZkIiwiZXhwIjoxNzM1MjM4MjI1LCJpc3MiOiJDTFRBUEkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MjcxIn0.iRtXYCMSErGTOWktd0J-t3PnJ7Bl66utquumJ_v_SVU
```

`GET` `/get-all-users`

`Use a paginate, by default PageSize is 10, you can filter by a {IsDeleted} or {IsBlocked} boolean status.`

```json
[
  {
    "userId": 2,
    "name": "Griselda SantaCruz",
    "email": "griseldasantacruz@gmail.com",
    "isDeleted": false,
    "isBlocked": false,
    "created_At": "18/12/2024",
    "updated_At": "18/12/2024"
  },
  {
    "userId": 15,
    "name": "Mauricio",
    "email": "mauricio@gmail.com",
    "isDeleted": false,
    "isBlocked": false,
    "created_At": "20/12/2024",
    "updated_At": "20/12/2024"
  }
]
```