# Guia Practica de Integracion con AuthStarter

## Objetivo

Esta guia explica, de forma practica, como otras aplicaciones pueden integrarse con la implementacion de autenticacion y sesion de este proyecto.

No busca detallar internamente todo el codigo ni la logica criptografica. El foco esta en el paso a paso que una aplicacion cliente necesita seguir para operar correctamente.

La guia se divide en dos escenarios:

1. aplicaciones nuevas, con tecnologia moderna o equivalente;
2. aplicaciones con tecnologia anterior, sistemas legados o con menor capacidad de automatizacion.

## Antes de comenzar

Toda aplicacion cliente debe considerar estas reglas:

- la API usa cookies para la sesion;
- la API protege operaciones mutantes con CSRF explicito;
- el `accesstoken` y el `refreshtoken` no se administran manualmente desde el cliente si este soporta cookies correctamente;
- la confirmacion de email es parte del alta de usuario;
- la sesion operativa completa requiere `Login`, seleccion de rol y `LoginOrganizacion`.

Endpoints base de referencia:

- `POST /api/account/register`
- `POST /api/account/login`
- `POST /api/account/loginorganizacion`
- `POST /api/account/refreshtoken`
- `POST /api/account/logout`
- `POST /api/account/confirmemail`
- `POST /api/account/resendconfirmationemail`
- `GET /api/account/csrf`
- `GET /api/account/miinformacion`
- `GET /api/account/currentuser`

---

## Seccion 1. Aplicaciones nuevas

Esta seccion aplica a:

- SPA modernas;
- frontends web con soporte de cookies;
- aplicaciones en Angular, React, Vue, Blazor WebAssembly, Next, Nuxt, etc.;
- clientes que pueden manejar `credentials/include`, interceptores, middlewares o request hooks.

### Paso 1. Configurar el cliente HTTP para usar cookies

El cliente debe enviar y recibir cookies en cada request.

En terminos practicos:

- habilitar envio de credenciales;
- mantener un cookie jar o un mecanismo equivalente;
- no intentar guardar manualmente el `accesstoken` y el `refreshtoken`.

Que hacer:

- si la tecnologia lo permite, usar una configuracion global del cliente HTTP;
- dejar la gestion de cookies al navegador o al runtime;
- no copiar tokens desde respuestas para guardarlos en almacenamiento local.

### Paso 2. Pedir el token CSRF al iniciar

Antes de llamar endpoints mutantes, la aplicacion debe ejecutar:

- `GET /api/account/csrf`

Resultado esperado:

- la API devuelve la cookie `XSRF-TOKEN`.

Que hacer:

- leer la cookie `XSRF-TOKEN`;
- enviar ese mismo valor en el header `X-CSRF-TOKEN` en cada `POST`, `PUT`, `PATCH` o `DELETE`.

Practica recomendada:

- resolver esto una sola vez en un interceptor o middleware de requests;
- no repetir la logica en cada pantalla o formulario.

### Paso 3. Registro de usuario

Para registrar:

- llamar `POST /api/account/register`;
- enviar CorreoElectronico, Nacionalidad, TipoDeUsuario, DocumentoDeIdentidad, NumeroDeDocumento, CodigoValidadorDocumento, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, SexoDeclarativo, SexoRegistral, FechaDeNacimiento, Contraseña, ConfirmaContraseña, TerminosYCondiciones;
- incluir cookie `XSRF-TOKEN` y header `X-CSRF-TOKEN`.

Resultado esperado:

- el usuario queda creado;
- no queda autenticado;
- la API indica que debe confirmar el email.

Que hacer en la aplicacion:

- mostrar un mensaje claro: "Debes confirmar tu correo antes de iniciar sesion";
- si estas en entorno de desarrollo, puedes usar el `confirmationUrl` devuelto para pruebas;
- si estas en produccion, esperar el enlace real enviado por correo.

### Paso 4. Confirmacion de email

La confirmacion puede resolverse de dos formas:

- desde un enlace recibido por correo;
- desde una pantalla que reciba `UserId` y `Token`.

Para completar la confirmacion:

- llamar `POST /api/account/confirmemail`;
- enviar `UserId` y `Token`;
- incluir CSRF.

Que hacer en la aplicacion:

- crear una pantalla simple de confirmacion;
- al recibir respuesta exitosa, redirigir a login.

### Paso 5. Login 1

Para iniciar sesion:

- llamar `POST /api/account/login`;
- enviar email y password;
- incluir CSRF.

Resultado esperado:

- la API entrega cookies de sesion;
- el usuario queda autenticado a nivel base;
- todavia no tiene sesion operativa final si debe seleccionar rol.

Que hacer despues:

- consultar `GET /api/account/currentuser` o `GET /api/account/miinformacion`;
- revisar si `SeleccionOrganizacionRequerida` viene en `true`.

### Paso 6. Seleccion de Organizacion y LoginOrganizacion

Si el usuario tiene mas de un rol operativo o la API exige completar el contexto:

- mostrar los roles disponibles;
- dejar que el usuario seleccione uno;
- llamar `POST /api/account/loginorganizacion` enviando el rol en el campo `Organizacion`.

Resultado esperado:

- la sesion queda rotada;
- se conserva el rol operativo seleccionado;
- la aplicacion ya puede navegar a funcionalidades protegidas.

Practica recomendada:

- modelar este paso como parte del onboarding de sesion;
- no mezclarlo con formularios complejos.

### Paso 7. Cargar estado de sesion al abrir la aplicacion

Cada vez que la aplicacion arranca:

1. obtiene o conserva CSRF;
2. llama `GET /api/account/currentuser`;
3. reconstruye el estado visual del usuario.

Esto evita:

- depender de memoria local inestable;
- perder el contexto al refrescar la pagina;
- duplicar logica de sesion en muchas pantallas.

### Paso 8. Refresh automatico

Si una llamada autenticada devuelve `401` por expiracion de sesion:

1. llamar `POST /api/account/refreshtoken`;
2. incluir CSRF;
3. reintentar la request original si el refresh fue exitoso.

Practica recomendada:

- manejarlo de forma centralizada;
- no pedir al usuario que vuelva a loguearse de inmediato si el refresh puede resolverlo.

### Paso 9. Logout

Para cerrar sesion:

- llamar `POST /api/account/logout`;
- incluir CSRF.

Resultado esperado:

- la API invalida la sesion;
- limpia cookies de autenticacion;
- el cliente vuelve a estado anonimo.

### Paso 10. Reenvio de confirmacion

Si el usuario no confirmo el email:

- llamar `POST /api/account/resendconfirmationemail`;
- enviar el correo;
- incluir CSRF.

Importante:

- la API ya tiene limites de frecuencia;
- si se recibe `429`, se debe mostrar un mensaje simple como "Espera unos minutos antes de volver a intentar".

### Recomendaciones para aplicaciones nuevas

- resolver cookies, CSRF y refresh en una capa comun;
- usar un servicio unico de autenticacion;
- exponer el estado de sesion desde un solo lugar;
- usar la respuesta de `currentuser` como contrato principal del usuario autenticado;
- no mover tokens manualmente si la plataforma ya soporta cookies correctamente.

---

## Seccion 2. Tecnologia anterior o sistemas legados

Esta seccion aplica a:

- aplicaciones MVC antiguas;
- WebForms;
- escritorios que consumen HTTP manualmente;
- servicios legados;
- integraciones con librerias antiguas sin manejo automatico de cookies o interceptores.

Aqui el objetivo no es modernizar todo de inmediato, sino lograr una integracion confiable con el menor cambio viable.

### Paso 1. Confirmar si el cliente soporta cookie jar

Primero debes saber si tu tecnologia:

- guarda cookies de respuesta automaticamente;
- las vuelve a enviar en requests posteriores;
- permite leer una cookie para copiar su valor a un header.

Si la respuesta es si:

- puedes seguir un flujo similar al de aplicaciones nuevas.

Si la respuesta es no:

- debes implementar un pequeno adaptador que capture cookies y las reenvie manualmente.

### Paso 2. Resolver manualmente el CSRF

En un sistema legado, este suele ser el primer punto critico.

Paso a paso:

1. ejecutar `GET /api/account/csrf`;
2. capturar la cookie `XSRF-TOKEN`;
3. conservar ese valor;
4. para cada request mutante, enviar:
   - la cookie `XSRF-TOKEN`
   - el header `X-CSRF-TOKEN` con el mismo valor

Si el cliente copia el valor directo desde `Set-Cookie`, debe usar el valor interpretado correctamente, no una forma codificada inconsistente.

### Paso 3. Centralizar cookies en un modulo comun

Si el sistema no tiene interceptores modernos, crea una capa pequeña y reutilizable para:

- leer cookies de respuestas;
- almacenarlas en memoria de sesion;
- reenviarlas en requests futuros.

No conviene repartir esta logica en distintos puntos del sistema legado.

### Paso 4. Separar claramente los tres momentos del flujo

En sistemas legados es importante no mezclar etapas:

1. confirmacion de correo;
2. `Login`;
3. `LoginOrganizacion`.

La recomendacion es modelarlos como pantallas o pasos distintos, incluso si la UI es muy simple.

### Paso 5. Registro y confirmacion

En un sistema antiguo, el flujo minimo recomendable es:

1. formulario de registro;
2. mensaje de confirmacion de correo;
3. pantalla o accion de confirmacion;
4. recien despues, login.

Si el sistema no puede procesar el enlace completo, una alternativa transitoria es:

- mostrar un formulario que acepte `UserId` y `Token`;
- invocar `POST /api/account/confirmemail`.

### Paso 6. LoginOrganizacion y seleccion de la Organizacion

No intentes ocultar `LoginOrganizacion` dentro de una sola llamada si el sistema legado no lo soporta bien.

Mejor:

1. ejecutar `login`;
2. consultar `currentuser`;
3. mostrar seleccion de la Organizacion;
4. ejecutar `loginOrganizacion`;
5. continuar con la operacion.

Es menos elegante, pero mucho mas estable en plataformas antiguas.

### Paso 7. Refresh en sistemas legados

Si el cliente no soporta refresco automatico elegante:

- detectar `401`;
- intentar una sola vez `POST /api/account/refreshtoken`;
- si resulta exitoso, reintentar la request original;
- si falla, redirigir a login.

No intentes automatizaciones demasiado sofisticadas si la plataforma no las resiste bien. En sistemas legados, la prioridad es previsibilidad.

### Paso 8. Manejar limites y mensajes simples

Para `resendconfirmationemail` y otros endpoints protegidos:

- manejar `429` con un mensaje claro y corto;
- no exponer detalles internos;
- registrar el evento para soporte si el sistema lo permite.

### Paso 9. Usar una pantalla o modulo de diagnostico

En integraciones legadas ayuda mucho tener una pantalla o log tecnico donde se pueda verificar:

- cookies presentes;
- ultimo valor de `XSRF-TOKEN`;
- estado del login;
- respuesta de `currentuser`.

Eso reduce mucho el tiempo de soporte cuando la integracion falla.

### Paso 10. Prioridad de implementacion para legados

Si no puedes hacer todo de una vez, implementa en este orden:

1. soporte de cookies;
2. soporte de CSRF;
3. login y logout;
4. currentuser;
5. loginorganizacion;
6. refresh;
7. confirmacion y reenvio de email.

Ese orden minimiza bloqueos y permite avanzar por etapas.

### Recomendaciones para sistemas legados

- evitar reescrituras grandes solo para integrarse;
- encapsular autenticacion en un modulo pequeno y estable;
- usar logs utiles para soporte;
- preferir flujos simples y visibles antes que automatismos fragiles;
- planificar una modernizacion gradual si la plataforma consume mucho esfuerzo solo para manejar sesion y cookies.

---

## Cierre

La implementacion de seguridad de este proyecto puede integrarse tanto con aplicaciones modernas como con sistemas legados, pero el esfuerzo y la comodidad cambian segun la capacidad del cliente para manejar cookies, CSRF y pasos de sesion.

La clave practica es esta:

- aplicaciones nuevas: automatizar casi todo;
- aplicaciones legadas: centralizar lo minimo indispensable y avanzar por capas.

Si se sigue este criterio, la integracion es viable sin necesidad de exponer internamente tokens ni debilitar el modelo de seguridad implementado.
