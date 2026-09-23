# Guía práctica de integración con AUT2Services

## Objetivo

Esta guía describe el flujo de autenticación convencional de AUT2Services para aplicaciones que consumen la API. Su propósito es permitir una integración segura con cookies, CSRF, selección de organización, renovación de sesión y cierre de sesión.

El alcance es la autenticación convencional. La validación de otros mecanismos de autenticación no forma parte de las pruebas documentadas aquí.

## Antes de comenzar

La aplicación cliente debe cumplir estas reglas:

- conservar y reenviar cookies entre solicitudes;
- pedir el token CSRF antes de cualquier operación mutante;
- copiar el valor de la cookie `XSRF-TOKEN` al encabezado `X-CSRF-TOKEN` para `POST`, `PUT` y `DELETE`;
- no guardar manualmente las cookies de autenticación en `localStorage`, `sessionStorage` ni archivos de configuración;
- completar el contexto de organización cuando la respuesta del login lo requiera.

Para desarrollo local, el perfil actual del backend expone la API en:

```text
http://localhost:5002
```

Antes de probar autenticación, compruebe disponibilidad con:

```text
GET /healthz
```

## Flujo de la sesión convencional

1. Solicitar `GET /api/account/csrf`.
2. Conservar la cookie `XSRF-TOKEN` que responde la API.
3. Ejecutar `POST /api/account/login` con correo y contraseña, incluyendo el encabezado CSRF.
4. Consultar `GET /api/account/currentuser`.
5. Si la respuesta solicita selección de organización, ejecutar `POST /api/account/loginorganizacion` con el código correspondiente.
6. Usar `POST /api/account/refreshtoken` solamente ante una expiración de sesión o de forma controlada por el cliente.
7. Cerrar sesión mediante `POST /api/account/logout`.

El backend administra las cookies de sesión. El cliente debe limitarse a permitir el cookie jar y a enviar el encabezado CSRF correcto.

## Pruebas con Postman

Los archivos se encuentran en:

- `docs/postman/AUT2Services.local.postman_collection.json`
- `docs/postman/AUT2Services.local.postman_environment.json`

La colección es una herramienta de apoyo para el flujo convencional y para APIs seleccionadas. No sustituye el contrato completo del backend ni las pruebas funcionales realizadas desde la aplicación web.

### Importar y preparar el ambiente

1. Inicie la API local y confirme `GET /healthz`.
2. Importe la colección y el ambiente local en Postman.
3. Seleccione el ambiente `AUT2Services Local`.
4. Verifique que `baseUrl` apunte al host local activo.
5. Use un ambiente privado local para credenciales de prueba. No publique contraseñas, cookies ni tokens en colecciones compartidas.

### Orden de ejecución

Ejecute las solicitudes de la carpeta **Auth Flow** en este orden:

1. **CSRF Bootstrap**: crea o conserva la cookie `XSRF-TOKEN` y guarda su valor para las solicitudes siguientes.
2. **Login**: valida el acceso convencional y registra las cookies que emite el backend.
3. **loginorganizacion**: ejecútela solo si el usuario debe completar el contexto operativo. La colección toma la organización obtenida desde la respuesta de login o permite definirla como variable.
4. **currentuser** o **miinformacion**: confirma que la sesión y el contexto esperado se encuentran disponibles.
5. **Refresh Token**: valida la renovación de sesión cuando corresponda.
6. **logout**: invalida la sesión al finalizar la prueba.

Las validaciones existentes de la colección deben mantenerse: verifican el estado HTTP esperado, guardan las cookies emitidas y detectan la ausencia de CSRF, token o organización antes de ejecutar una solicitud dependiente.

## Aplicaciones modernas

En una SPA, frontend web o cliente HTTP moderno:

- configure el cliente con envío de credenciales;
- centralice el manejo de CSRF en un interceptor o servicio HTTP;
- reconstruya el estado de sesión con `currentuser` al iniciar la aplicación;
- ante un `401`, intente un refresh una sola vez y redirija a login si falla;
- mantenga el flujo de selección de organización como una pantalla o estado explícito.

## Sistemas legados

Cuando el cliente no dispone de interceptores ni cookie jar automático:

1. implemente un módulo único para guardar y reenviar cookies;
2. obtenga primero `XSRF-TOKEN` y envíelo también como encabezado;
3. mantenga separadas las etapas de login, selección de organización y refresh;
4. registre solo información sanitizada para soporte, sin cookies ni tokens;
5. trate `401` como señal para un único intento de refresh y `429` como una condición de espera antes de reintentar.

## Errores frecuentes

- Ejecutar un `POST` sin haber solicitado CSRF.
- Enviar un `X-CSRF-TOKEN` distinto de la cookie `XSRF-TOKEN`.
- No conservar cookies entre login, selección de organización y refresh.
- Asumir que el registro deja al usuario autenticado.
- Omitir la selección de organización cuando la API la solicita.
- Cambiar el `baseUrl` de Postman sin confirmar antes el endpoint `/healthz`.

## Cierre

La integración es estable cuando el cliente conserva cookies, aplica CSRF de forma centralizada y trata la selección de organización como parte explícita de la sesión. La colección Postman permite comprobar este flujo de forma repetible sin exponer secretos ni alterar la validación ya implementada.
