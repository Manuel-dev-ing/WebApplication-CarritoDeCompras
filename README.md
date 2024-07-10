# LibrosNet

 🚧Proyecto en construcion 🚧

## Descripcion
LibrosNet es una una tienda de libros gratis y de codigo abierto construido en ASP.NET core, aprovechando las ultimas mejoras de esta tecnologia para ofrecer seguridad y escalabilidad. Diseñado en la arquitectura MVC permitiendo futuras actulizaciones y mejoras sin interupciones significativas.   

## Funcionalidades

- `funcionalidad 1:` Autenticacion y Autorizacion con Identity .

- `funcionalidad 2:` Registros de nuevos usuarios y autenticacion de usuarios existentes.

- `funcionalidad 3:` Administracion de Cuentas de Usuarios incluyendo la capacidad de crear, eliminar, modificar y filtrar usuarios.

- `funcionalidad 4:` Administracion de roles de usuarios incluyendo la capacidad de crar, eliminar, modificar roles a los usuarios.

- `funcionalidad 5:` Carrito de Compras, Añanadir al carrito, Gestion del carrito y calculo automatico el cual permite calcular automaticamente el total de la compra.     

- `funcionalidad 6:` Categorizacion de libros y Filtrar libros por categoria.

- `funcionalidad 7:` Acceso a usuarios con rol de Administrador.

- `funcionalidad 8:` Busqueda y filtro avanzado permite buscar por autor, categoria, precio y mas.

- `funcionalidad 9:` Paginacion para vista de libros, autores categorias y ventas.


## Tecnologias utilizadas

- C#
- ASP.NET CORE 
- RAZOR PAGES
- Dapper
- SQL Server
- CSS
- JavaScript
- .NET 8

## Instalacion
Asegurate de tener instalados los siguientes coponentes en tu maquina:

1. SDK de .NET 8: puedesPuedes descargarlo e instalarlo desde dotnet.microsoft.com.

2. Visual Studio.

3. SQL Server (opcional, si usas otra base de datos, asegúrate de tenerla configurada).

### Pasos de Instalación
1. Clonar el Repositorio

Clona el repositorio de GitHub en tu máquina local:
```
git clone https://github.com/tuusuario/booknest.git

```

2. Navegar al directorio del proyecto:

Ve al directorio del proyecto:
```
    cd librosNet
```
3. Configurar la Base de Datos

Si estás usando SQL Server, crea una base de datos para la aplicación. Por ejemplo, puedes usar SQL Server Management Studio (SSMS) para crear una base de datos llamada librosNet.

4. Configurar las Variables de Entorno

Crea un archivo appsettings.Development.json en el directorio raíz del proyecto y configura las cadenas de conexión de la base de datos:

```
{
  "ConnectionStrings": {
    "DefaultConnection":"Server=localhost;Database=BookNestDB;User Id=tu_usuario;Password=tu_contraseña;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

```

### Notas Adicionales

- Depuración: Puedes abrir el proyecto en Visual Studio y utilizar sus herramientas de depuración para correr y depurar la aplicación.
- Configuración Adicional: Asegúrate de revisar y configurar cualquier otro archivo de configuración o dependencias específicas que tu proyecto pueda tener.


## Licencia

LibrosNet es [MIT licensed](./LICENSE).

## Contacto
**Nombre:** Manuel Tamayo Montero.

**Correo:** manueltamayo9765@gmail.com