# ecommerce-crud-demo

Una aplicación web desarrollada en .NET MVC que simula ser un sitio de E-Commerce. Incluye productos almacenados en una base de datos SQL (permite la creación de nuevos y la edición o borrado de los existentes), control de stock automático, carrito de compras por usuario y diferentes filtros de búsqueda de productos. Incluye una API simple que devuelve productos por ID (instrucciones en la pestaña "acerca de" dentro de la aplicación)

Frontend -> HTML - CSS - JavaScript - (Bootstrap 4.x, JQuery 3.x).
Backend -> .NET Framework 4.5 (C#) -> Entity Framework 6 (Code First) -> SQL Server.


## IMPORTANTE
Solución desarrollada en Visual Studio 2017. Requiere de base de datos local instalada para su uso. (https://stackoverflow.com/questions/42774739/how-to-install-localdb-2016-along-with-visual-studio-2017).


### Funcionalidades destacadas

* Autenticación por medio de cuentas individuales (Identity) con roles de usuario y administrador. La base de datos provista tiene ya guardados una cuenta con rol de usuario y otra con rol de administrador, los datos de acceso pueden verse en la página de inicio de sesión dentro de la aplicación. Todas las cuentas nuevas que se registren se clasifican automáticamente como usuarios.

* Pestaña exclusiva para administradores que permite agregar, quitar o editar productos.

* Posibilidad de ordenar los productos según diferentes criterios, barra de búsqueda por nombre.

* Carrito de compras diferenciado según usuario anónimo (guardado de datos por sesión) y usuario registrado (el carrito es guardado en la base de datos). Checkeo de stock al momento de la compra.

