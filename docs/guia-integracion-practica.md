# Guia Practica de Integracion con AuthStarter

## Objetivo

Esta guia explica, de forma practica y semi tecnica, como una aplicacion cliente debe integrarse con la autenticacion y gestion de sesion implementada en este proyecto.

No busca documentar todo el codigo interno ni la logica criptografica. El foco esta en el flujo que debe seguir una aplicacion para registrarse, iniciar sesion, seleccionar contexto operativo, mantener la sesion y cerrarla correctamente.

La guia se divide en dos escenarios:

1. aplicaciones nuevas, con tecnologia moderna o equivalente;
2. aplicaciones con tecnologia anterior, sistemas legados o con menor capacidad de automatizacion.

## Antes de comenzar

Toda aplicacion cliente debe considerar estas reglas:

- la API usa cookies para transportar la sesion;
- las operaciones mutantes requieren CSRF explicito;
- el cliente no debe administrar manualmente `accesstoken` ni `refreshtoken` si puede usar cookies correctamente;
- la confirmacion de email forma parte del alta de usuario;
- la sesion operativa puede requerir dos pasos: `Login` y `LoginOrganizacion`;
- tambien existe login externo por ClaveUnica mediante `LoginClaveUnica`;
- el estado visible de sesion debe reconstruirse usando `currentuser`.

Endpoints base de referencia:

- `POST /api/account/register`
- `POST /api/account/login`
- `POST /api/account/loginclaveunica`
- `POST /api/account/loginorganizacion`
- `POST /api/account/cambiounidadorganizacionalentidadrol`
- `POST /api/account/refreshtoken`
- `POST /api/account/logout`
- `POST /api/account/confirmemail`
- `POST /api/account/resendconfirmationemail`
- `POST /api/account/solicitarecuperacionclave`
- `POST /api/account/reenviacodigorecuperacionclave`
- `POST /api/account/confirmarecuperacionclave`
- `GET /api/account/csrf`
- `GET /api/account/currentuser`
- `GET /api/account/miinformacion`

---

## Flujo recomendado de sesion

Para la mayoria de las aplicaciones, el flujo correcto es:

1. llamar `GET /api/account/csrf`;
2. ejecutar `POST /api/account/login` o `POST /api/account/loginclaveunica`;
3. consultar `GET /api/account/currentuser`;
4. si la respuesta indica seleccion de organizacion requerida, ejecutar `POST /api/account/loginorganizacion`;
5. reconstruir el estado visual con `currentuser`;
6. ante expiracion, ejecutar `POST /api/account/refreshtoken` una sola vez y reintentar la operacion original;
7. cerrar sesion con `POST /api/account/logout`.

Despues de operaciones como login, login de organizacion o refresh, el cliente debe seguir confiando en cookies. Si la cookie `XSRF-TOKEN` cambia, debe usar el nuevo valor para las siguientes operaciones mutantes.

---

## Seccion 1. Aplicaciones nuevas

Esta seccion aplica a:

- SPA modernas;
- frontends web con soporte de cookies;
- aplicaciones en Angular, React, Vue, Blazor WebAssembly, Next, Nuxt, etc.;
- clientes que pueden manejar `withCredentials`, `credentials: 'include'`, interceptores, middlewares o request hooks.

### Paso 1. Configurar el cliente HTTP para usar cookies

El cliente debe enviar y recibir cookies en cada request.

En terminos practicos:

- habilitar envio de credenciales;
- mantener un cookie jar o mecanismo equivalente;
- no copiar tokens desde respuestas hacia `localStorage`, `sessionStorage` ni variables globales de frontend.

Que hacer:

- si la tecnologia lo permite, usar una configuracion global del cliente HTTP;
- dejar la gestion de cookies al navegador o al runtime;
- centralizar la autenticacion en un servicio comun.

### Paso 2. Pedir CSRF al iniciar

Antes de llamar endpoints mutantes, la aplicacion debe ejecutar:

- `GET /api/account/csrf`

Resultado esperado:

- la API asegura la cookie `XSRF-TOKEN`;
- la respuesta puede no traer contenido util en el body.

Que hacer:

- leer la cookie `XSRF-TOKEN`;
- enviar ese mismo valor en el header `X-CSRF-TOKEN` en cada `POST`, `PUT`, `PATCH` o `DELETE`.

Practica recomendada:

- resolver esto una sola vez en un interceptor o middleware de requests;
- no repetir la logica en cada pantalla o formulario;
- si cambia la cookie `XSRF-TOKEN`, usar siempre el valor mas reciente.

### Paso 3. Registro de usuario

Para registrar:

- llamar `POST /api/account/register`;
- enviar CorreoElectronico, Nacionalidad, TipoDeUsuario, DocumentoDeIdentidad, NumeroDeDocumento, CodigoValidadorDocumento, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, SexoDeclarativo, SexoRegistral, FechaDeNacimiento, Contraseña, ConfirmaContraseña y TerminosYCondiciones;
- incluir cookie `XSRF-TOKEN` y header `X-CSRF-TOKEN`.

Resultado esperado:

- el usuario queda creado;
- no queda autenticado automaticamente;
- la API indica que debe confirmar el email.

Que hacer en la aplicacion:

- mostrar un mensaje claro: "Debes confirmar tu correo antes de iniciar sesion";
- si estas en entorno de desarrollo, puedes usar la URL de confirmacion devuelta para pruebas;
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
- al recibir respuesta exitosa, redirigir a login;
- si el token expiro o ya fue usado, mostrar una salida controlada y ofrecer reenvio.

### Paso 5. Login con email y password

Para iniciar sesion tradicional:

- llamar `POST /api/account/login`;
- enviar `email` y `password`;
- incluir CSRF.

Resultado esperado:

- la API entrega cookies de sesion;
- el usuario queda autenticado a nivel base;
- todavia puede faltar seleccionar organizacion o contexto operativo.

Que hacer despues:

- consultar `GET /api/account/currentuser`;
- revisar si la respuesta indica `seleccionOrganizacionRequerida`;
- si corresponde, mostrar las opciones disponibles y continuar con `LoginOrganizacion`.

### Paso 6. Login con ClaveUnica

Para login externo con ClaveUnica:

- completar primero el flujo propio de ClaveUnica hasta obtener `code` y `state`;
- llamar `POST /api/account/loginclaveunica`;
- enviar `clientId`, `redirectUri`, `code` y `state`;
- incluir CSRF.

Resultado esperado:

- si la validacion externa es correcta, la API emite cookies de sesion;
- el flujo posterior es el mismo que en login tradicional;
- puede requerir seleccion de organizacion antes de quedar operativa.

Que hacer en la aplicacion:

- tratar `loginclaveunica` como otro mecanismo de entrada, no como una sesion distinta;
- despues de la respuesta, llamar `currentuser`;
- si falta contexto, ejecutar `loginorganizacion`.

### Paso 7. Seleccion de organizacion y LoginOrganizacion

Si el usuario tiene mas de una opcion operativa o la API exige completar el contexto:

- mostrar las organizaciones o roles disponibles;
- dejar que el usuario seleccione una opcion;
- llamar `POST /api/account/loginorganizacion` enviando el codigo seleccionado en el campo `Organizacion`.

Resultado esperado:

- la sesion queda asociada al contexto operativo seleccionado;
- se conserva el rol, organizacion y unidad operativa vigentes;
- la aplicacion ya puede navegar a funcionalidades protegidas.

Practica recomendada:

- modelar este paso como parte del onboarding de sesion;
- no ocultarlo dentro de formularios complejos;
- recargar `currentuser` despues de completarlo.

### Paso 8. Cambio de unidad, entidad o rol durante la sesion

Si la aplicacion permite cambiar de contexto sin cerrar sesion, debe usar:

- `POST /api/account/cambiounidadorganizacionalentidadrol`

Este endpoint aplica cuando el usuario ya esta autenticado y necesita cambiar unidad organizacional, entidad o rol operativo.

Que hacer:

- incluir CSRF;
- enviar los identificadores requeridos por la opcion seleccionada;
- actualizar el estado visual llamando nuevamente `GET /api/account/currentuser`;
- invalidar o recargar datos de pantalla que dependan del contexto anterior.

No conviene simular este cambio cerrando y abriendo sesion si existe este endpoint disponible.

### Paso 9. Cargar estado de sesion al abrir la aplicacion

Cada vez que la aplicacion arranca:

1. obtiene o conserva CSRF;
2. llama `GET /api/account/currentuser`;
3. reconstruye el estado visual del usuario.

Esto evita:

- depender de memoria local inestable;
- perder el contexto al refrescar la pagina;
- duplicar logica de sesion en muchas pantallas.

La respuesta puede incluir datos como:

- expiracion de access token;
- si falta seleccion de organizacion;
- organizacion seleccionada;
- unidad organizacional seleccionada;
- entidad seleccionada;
- unidades organizacionales disponibles para el usuario.

### Paso 10. Refresh automatico

Si una llamada autenticada devuelve `401` por expiracion de sesion:

1. llamar `POST /api/account/refreshtoken`;
2. incluir CSRF;
3. si el refresh fue exitoso, reintentar la request original una sola vez;
4. si falla, limpiar estado local y redirigir a login.

Practica recomendada:

- manejarlo de forma centralizada;
- evitar ciclos infinitos de refresh;
- no pedir al usuario que vuelva a loguearse si el refresh puede resolverlo;
- actualizar el CSRF desde cookie si la API lo rota.

### Paso 11. Logout

Para cerrar sesion:

- llamar `POST /api/account/logout`;
- incluir CSRF.

Resultado esperado:

- la API invalida la sesion;
- limpia cookies de autenticacion;
- el cliente vuelve a estado anonimo.

Despues del logout:

- limpiar estado visual local;
- eliminar caches de datos protegidos;
- redirigir a login o pantalla publica.

### Paso 12. Reenvio de confirmacion y recuperacion de clave

Si el usuario no confirmo el email:

- llamar `POST /api/account/resendconfirmationemail`;
- enviar el correo;
- incluir CSRF.

Si el usuario olvido su clave:

- iniciar con `POST /api/account/solicitarecuperacionclave`;
- si corresponde, usar `POST /api/account/reenviacodigorecuperacionclave`;
- completar con `POST /api/account/confirmarecuperacionclave`.

Importante:

- estos endpoints pueden tener limites de frecuencia;
- si se recibe `429`, mostrar un mensaje simple como "Espera unos minutos antes de volver a intentar";
- no exponer detalles internos de validacion ni seguridad.

### Recomendaciones para aplicaciones nuevas

- resolver cookies, CSRF y refresh en una capa comun;
- usar un servicio unico de autenticacion;
- exponer el estado de sesion desde un solo lugar;
- usar `currentuser` como contrato principal del usuario autenticado o anonimo con sesion ausente;
- no mover tokens manualmente si la plataforma ya soporta cookies correctamente;
- tratar ClaveUnica como una variante de login, no como un segundo modelo de sesion;
- recargar datos dependientes cuando cambie organizacion, unidad, entidad o rol.

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
   - la cookie `XSRF-TOKEN`;
   - el header `X-CSRF-TOKEN` con el mismo valor.

Si el cliente copia el valor directo desde `Set-Cookie`, debe usar el valor interpretado correctamente, no una forma codificada inconsistente.

### Paso 3. Centralizar cookies en un modulo comun

Si el sistema no tiene interceptores modernos, crea una capa pequena y reutilizable para:

- leer cookies de respuestas;
- almacenarlas en memoria de sesion;
- reenviarlas en requests futuros;
- actualizar `XSRF-TOKEN` cuando la API lo vuelva a emitir.

No conviene repartir esta logica en distintos puntos del sistema legado.

### Paso 4. Separar claramente los momentos del flujo

En sistemas legados es importante no mezclar etapas:

1. confirmacion de correo;
2. `Login` o `LoginClaveUnica`;
3. `LoginOrganizacion`, si aplica;
4. cambio de contexto, si la aplicacion lo permite;
5. refresh o logout.

La recomendacion es modelarlos como pantallas, acciones o pasos distintos, incluso si la UI es muy simple.

### Paso 5. Registro y confirmacion

En un sistema antiguo, el flujo minimo recomendable es:

1. formulario de registro;
2. mensaje de confirmacion de correo;
3. pantalla o accion de confirmacion;
4. recien despues, login.

Si el sistema no puede procesar el enlace completo, una alternativa transitoria es:

- mostrar un formulario que acepte `UserId` y `Token`;
- invocar `POST /api/account/confirmemail`.

### Paso 6. LoginOrganizacion y seleccion de organizacion

No intentes ocultar `LoginOrganizacion` dentro de una sola llamada si el sistema legado no lo soporta bien.

Mejor:

1. ejecutar `login` o `loginclaveunica`;
2. consultar `currentuser`;
3. mostrar seleccion de organizacion;
4. ejecutar `loginorganizacion`;
5. volver a consultar `currentuser`;
6. continuar con la operacion.

Es menos elegante, pero mucho mas estable en plataformas antiguas.

### Paso 7. Cambio de contexto en sistemas legados

Si el sistema legado permite cambiar unidad, entidad o rol durante la sesion:

- usar `POST /api/account/cambiounidadorganizacionalentidadrol`;
- incluir CSRF y cookies vigentes;
- recargar `currentuser` despues del cambio;
- limpiar datos temporales asociados al contexto anterior.

Si no puede garantizar esa limpieza, es preferible forzar una navegacion controlada o recarga completa de la pantalla.

### Paso 8. Refresh en sistemas legados

Si el cliente no soporta refresco automatico elegante:

- detectar `401`;
- intentar una sola vez `POST /api/account/refreshtoken`;
- si resulta exitoso, reintentar la request original;
- si falla, redirigir a login.

No intentes automatizaciones demasiado sofisticadas si la plataforma no las resiste bien. En sistemas legados, la prioridad es previsibilidad.

### Paso 9. Manejar limites y mensajes simples

Para reenvio de confirmacion, recuperacion de clave y otros endpoints protegidos por limites:

- manejar `429` con un mensaje claro y corto;
- no exponer detalles internos;
- registrar el evento para soporte si el sistema lo permite.

### Paso 10. Usar una pantalla o modulo de diagnostico

En integraciones legadas ayuda mucho tener una pantalla o log tecnico donde se pueda verificar:

- cookies presentes;
- ultimo valor de `XSRF-TOKEN`;
- estado del login;
- respuesta de `currentuser`;
- organizacion, unidad, entidad y rol actualmente seleccionados.

Eso reduce mucho el tiempo de soporte cuando la integracion falla.

### Paso 11. Prioridad de implementacion para legados

Si no puedes hacer todo de una vez, implementa en este orden:

1. soporte de cookies;
2. soporte de CSRF;
3. login y logout;
4. currentuser;
5. loginorganizacion;
6. refresh;
7. cambio de contexto;
8. confirmacion y reenvio de email;
9. recuperacion de clave;
10. ClaveUnica, si el sistema la requiere.

Ese orden minimiza bloqueos y permite avanzar por etapas.

### Recomendaciones para sistemas legados

- evitar reescrituras grandes solo para integrarse;
- encapsular autenticacion en un modulo pequeno y estable;
- usar logs utiles para soporte;
- preferir flujos simples y visibles antes que automatismos fragiles;
- planificar una modernizacion gradual si la plataforma consume mucho esfuerzo solo para manejar sesion, cookies y CSRF.

---

## Cierre

La implementacion de seguridad de este proyecto puede integrarse tanto con aplicaciones modernas como con sistemas legados, pero el esfuerzo y la comodidad cambian segun la capacidad del cliente para manejar cookies, CSRF y pasos de sesion.

La clave practica es esta:

- aplicaciones nuevas: automatizar cookies, CSRF, refresh y reconstruccion de estado;
- aplicaciones legadas: centralizar lo minimo indispensable y avanzar por capas.

Si se sigue este criterio, la integracion es viable sin exponer tokens manualmente ni debilitar el modelo de seguridad implementado.
