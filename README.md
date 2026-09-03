# ReservasApp

Sistema de gestión de reservas de aulas desarrollado en WPF (.NET) con ADO.NET, correspondiente al laboratorio "ADO .NET Semana 03".

## Datos del curso

- **Curso**: Desarrollo de Aplicaciones Empresariales Avanzado
- **Sección**: 6C24-C
- **Docente**: Edwin William Arévalo Sermeño

## Integrantes

- Medina Mallqui, Ailyn
- Ochoa Marín, Yamile

## Descripción

La aplicación permite iniciar sesión, consultar aulas y reservas, buscarlas por nombre o por fecha, y registrar nuevas reservas validando que no exista un conflicto de horario. El proyecto implementa las dos formas de acceso a datos de ADO.NET (conectado y desconectado) y el patrón MVVM en los módulos de Aulas y Reservas.

## Tecnologías

- .NET / WPF — interfaz de escritorio
- SQL Server — base de datos relacional
- Microsoft.Data.SqlClient — acceso a datos ADO.NET
- System.Configuration.ConfigurationManager — lectura del connection string
- MVVM implementado manualmente (INotifyPropertyChanged e ICommand propios, sin frameworks externos)

## Base de datos

Base de datos `ReservasDB` con tres tablas:

| Tabla | Campos | Descripción |
|---|---|---|
| Usuarios | UsuarioId (PK), Username, Password, NombreCompleto | Cuentas para el inicio de sesión |
| Aulas | AulaId (PK), Nombre, Capacidad | Ambientes disponibles para reservar |
| Reservas | ReservaId (PK), AulaId (FK), UsuarioId (FK), Fecha, Hora, Motivo | Reservas realizadas sobre un aula |

`Reservas` se relaciona con `Aulas` y `Usuarios` mediante llaves foráneas: cada reserva pertenece a un aula y fue registrada por un usuario.

## Estructura del proyecto

```
ReservasApp/
├── Models/        Usuario, Aula, Reserva
├── Helpers/       ConexionHelper, SesionActual, ObservableObject, RelayCommand
├── ViewModels/    Lógica y acceso a datos de Aulas y Reservas
├── Views/         Login, Menú, vistas de Aulas/Reservas, Nueva Reserva
├── App.xaml
└── App.config     Connection string a SQL Server
```

## Explicación del desarrollo

El proyecto se construyó en el siguiente orden, de manera que cada parte se apoyara en la anterior.

**1. Base de datos.** Se creó primero `ReservasDB`, con las tablas `Usuarios`, `Aulas` y `Reservas` y sus datos de prueba, ya que todo el resto del proyecto depende de que estas tablas existan.

**2. Proyecto y estructura de carpetas.** Se creó el proyecto WPF en Visual Studio y se organizó en cuatro carpetas — `Models`, `ViewModels`, `Views` y `Helpers` — para separar responsabilidades desde el inicio, en lugar de tener todo el código dentro de las ventanas.

**3. Configuración y conexión (App.config y Helpers).** Se definió el connection string en `App.config`, bajo el nombre `ReservasDB`. Sobre esa configuración se construyó `ConexionHelper`, una clase que centraliza la creación de conexiones a SQL Server: cualquier parte del programa que necesite hablar con la base de datos pide una conexión a esta clase, en lugar de construirla por su cuenta. Esto evita repetir el string de conexión en cada archivo.

**4. Modelos.** Se definieron las clases `Usuario`, `Aula` y `Reserva` en `Models`, con una propiedad por cada columna de su tabla correspondiente. Son clases sin lógica: solo representan los datos que se leen o se escriben en la base de datos.

**5. Login.** Con la conexión y el modelo `Usuario` ya disponibles, se construyó la ventana de inicio de sesión. Al presionar "Ingresar", se abre una conexión y se ejecuta una consulta parametrizada contra `Usuarios` con un `SqlDataReader`, mientras la conexión permanece abierta (acceso conectado). Si las credenciales coinciden, el `UsuarioId` y el nombre del usuario se guardan en `SesionActual` — una clase que actúa como "sesión" en memoria durante toda la ejecución — y se abre el menú principal; si no, se muestra un error en la misma ventana.

**6. Infraestructura de MVVM (Helpers).** Antes de construir las vistas de Aulas y Reservas, se agregaron dos clases base en `Helpers`: `ObservableObject`, que permite que un ViewModel avise a su Vista cuando una propiedad cambia, y `RelayCommand`, que permite que un botón del XAML ejecute un método del ViewModel sin usar el evento `Click` tradicional. Estas dos clases son la base que hace posible MVVM en el resto del proyecto.

**7. Aulas (DataTable y Objetos).** Se construyeron dos ViewModels para Aulas: uno que llena un `DataTable` con `SqlDataAdapter.Fill` (acceso desconectado: la conexión se abre y se cierra sola al traer los datos), y otro que recorre un `SqlDataReader` fila por fila construyendo una lista de objetos `Aula` (acceso conectado), con un comando de búsqueda por nombre. Sus vistas correspondientes solo declaran un `DataGrid` enlazado (`{Binding}`) a la lista que expone el ViewModel.

**8. Reservas (DataTable y Objetos).** Se repitió el mismo patrón que en Aulas, agregando un `INNER JOIN` con `Aulas` y `Usuarios` para mostrar nombres en vez de identificadores, y un comando de búsqueda por fecha en la versión de objetos.

**9. Nueva Reserva.** Se construyó el formulario de registro con su propio ViewModel, que antes de insertar ejecuta una consulta de verificación (`SELECT COUNT(*)`) sobre la misma combinación de aula, fecha y hora. Si ya existe una reserva así, se informa al usuario y se cancela el guardado; si no, se procede con el `INSERT`.

**10. Menú principal.** Como último paso, se construyó la ventana de menú, que recibe los datos de `SesionActual` para personalizar el saludo y abre cada una de las ventanas anteriores desde sus respectivos botones.

En los módulos de Aulas y Reservas, el resultado final de este orden es una separación clara: el **Model** representa los datos, el **ViewModel** contiene las consultas SQL y expone propiedades y comandos, y la **View** solo declara controles enlazados a su ViewModel, sin lógica de negocio propia.

## Observaciones y conclusiones

- Los Helpers nos ayudaron bastante, porque evitamos repetir la conexión a la base de datos en cada ventana. Además, si necesitábamos cambiar de servidor, podíamos hacerlo de una manera más sencilla sin modificar todo el código.

- Durante el desarrollo tuvimos algunas dificultades, principalmente al manejar las conexiones y al separar la lógica de la interfaz. Pero esto nos ayudó a entender mejor cómo organizar el proyecto y cómo solucionar los problemas que iban apareciendo.

- También aprendimos la diferencia entre el modo conectado y desconectado. En el modo conectado se trabaja directamente con la base de datos y los datos se manejan en tiempo real. En cambio, en el modo desconectado los datos se cargan y se almacenan temporalmente en memoria, sin necesidad de mantener la conexión abierta.

- El uso de MVVM hizo que el proyecto estuviera más ordenado, porque separamos la parte visual de la lógica del programa. Esto facilita hacer cambios y mantener el código.

- Finalmente, usar parámetros y validar las reservas fue importante, ya que nos ayudó a evitar problemas de seguridad, como la inyección SQL, y también reservas duplicadas. Además, tener una estructura ordenada facilitó el trabajo en equipo.
