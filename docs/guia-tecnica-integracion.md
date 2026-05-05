# Guia para Equipos Tecnicos de Integracion

## Objetivo

Esta guia entrega una referencia tecnica para equipos que deben integrar, mantener o extender clientes que consumen la seguridad implementada en AUT2Services.

El foco esta en integracion, contratos y comportamiento observable del sistema, no en detalle interno de codigo.

---

## Principios de integracion

- la sesion se transporta mediante cookies (`accesstoken`, `refreshToken`);
- las operaciones mutantes requieren CSRF (`XSRF-TOKEN` + `X-CSRF-TOKEN`);
- el cliente no debe persistir manualmente tokens si puede usar cookies;
- la sesion puede requerir dos etapas: autenticacion base y fijacion de contexto (`LoginOrganizacion`);
- existe autenticacion externa mediante ClaveUnica (`LoginClaveUnica`);
- el estado de sesion se reconstruye usando `GET /api/account/currentuser`.

---

## Endpoints principales

### Autenticacion y sesion

- `POST /api/account/register`
- `POST /api/account/login`
- `POST /api/account/loginclaveunica`
- `POST /api/account/loginorganizacion`
- `POST /api/account/cambiounidadorganizacionalentidadrol`
- `POST /api/account/refreshtoken`
- `POST /api/account/logout`

### Email y validacion

- `POST /api/account/confirmemail`
- `POST /api/account/resendconfirmationemail`

### Recuperacion de clave

- `POST /api/account/solicitarecuperacionclave`
- `POST /api/account/reenviacodigorecuperacionclave`
- `POST /api/account/confirmarecuperacionclave`

### Estado de sesion

- `GET /api/account/csrf`
- `GET /api/account/currentuser` (AllowAnonymous)
- `GET /api/account/miinformacion` (Authorize)

---

## Flujo tecnico de integracion

### 1. Inicializacion

- llamar `GET /api/account/csrf`;
- capturar cookie `XSRF-TOKEN`;
- configurar cliente HTTP para enviar cookies automaticamente.

### 2. CSRF

Regla obligatoria para endpoints mutantes:

- enviar cookie `XSRF-TOKEN`;
- enviar header `X-CSRF-TOKEN` con el mismo valor.

Notas:

- el endpoint `csrf` puede responder sin body (204);
- la cookie puede rotar, el cliente debe usar siempre el ultimo valor;
- errores de CSRF se manifiestan como 400/403.

### 3. Registro

Contrato:

- endpoint: `POST /api/account/register`
- payload: datos personales + password + terminos
- requiere CSRF

Resultado:

- usuario creado;
- sin sesion;
- email debe ser confirmado.

### 4. Confirmacion de email

Contrato:

- endpoint: `POST /api/account/confirmemail`
- payload: `UserId`, `Token`
- requiere CSRF

### 5. Login

#### Login tradicional

- endpoint: `POST /api/account/login`
- payload: `email`, `password`
- requiere CSRF

Resultado:

- cookies de sesion emitidas;
- puede requerir seleccion de organizacion.

#### Login ClaveUnica

- endpoint: `POST /api/account/loginclaveunica`
- payload: `clientId`, `redirectUri`, `code`, `state`
- requiere CSRF

Resultado:

- cookies emitidas si validacion externa es correcta;
- flujo posterior identico al login tradicional.

### 6. Estado de sesion

- endpoint: `GET /api/account/currentuser`

Usos:

- reconstruir estado al iniciar app;
- detectar autenticacion;
- detectar si falta seleccion de organizacion;
- obtener roles, organizaciones y unidades disponibles.

Importante:

- este endpoint es AllowAnonymous;
- si hay cookies validas, devuelve contexto;
- si no, devuelve estado anonimo.

### 7. LoginOrganizacion

Contrato:

- endpoint: `POST /api/account/loginorganizacion`
- payload: `Organizacion`
- requiere CSRF

Resultado:

- fija organizacion/rol operativo;
- rota sesion;
- invalida refresh token anterior.

### 8. Cambio de contexto

Contrato:

- endpoint: `POST /api/account/cambiounidadorganizacionalentidadrol`
- requiere autorizacion y CSRF

Uso:

- cambiar unidad organizacional, entidad o rol sin cerrar sesion;
- debe ir seguido de una llamada a `currentuser` para sincronizar estado.

### 9. Refresh

Contrato:

- endpoint: `POST /api/account/refreshtoken`
- requiere CSRF

Flujo:

1. detectar `401`;
2. ejecutar refresh una sola vez;
3. si es exitoso, reintentar request original;
4. si falla, invalidar estado local y forzar login.

Consideraciones:

- mantiene contexto operativo;
- puede rotar cookies y CSRF;
- no debe ejecutarse en bucle.

### 10. Logout

Contrato:

- endpoint: `POST /api/account/logout`
- requiere CSRF

Resultado:

- invalida sesion en servidor;
- elimina cookies;
- cliente debe limpiar estado.

---

## Ejemplo Angular (corregido)

Puntos clave:

- usar `GET /api/account/csrf` (no rutas mal escritas);
- usar `withCredentials: true`;
- no copiar cookies manualmente;
- enviar `X-CSRF-TOKEN` correctamente.

La logica de CSRF debe moverse idealmente a un interceptor.

---

## Manejo de errores

Casos relevantes:

- `401`: sesion expirada -> intentar refresh;
- `403` o `400`: error CSRF;
- `429`: rate limiting en resend o recuperacion;
- login bloqueado si email no confirmado.

---

## Buenas practicas de mantenimiento

- centralizar autenticacion en un unico modulo;
- no duplicar rutas de endpoints en multiples servicios;
- usar `currentuser` como unica fuente de verdad del estado;
- refrescar estado despues de loginorganizacion y cambio de contexto;
- evitar depender de datos cacheados cuando cambian cookies;
- monitorear respuestas inconsistentes de backend y normalizarlas en cliente.

---

## Errores frecuentes

- usar rutas antiguas como `/login-2` o `/refresh`;
- olvidar `GET /csrf` antes de POST;
- no sincronizar cookie y header CSRF;
- no reenviar cookies;
- asumir que login deja la sesion completa lista;
- no manejar correctamente `currentuser`;
- ignorar `429` en endpoints protegidos por rate limiting.

---

## Resumen

La integracion correcta depende de tres capacidades clave:

1. manejo de cookies;
2. manejo correcto de CSRF;
3. soporte para flujo de sesion en dos etapas y cambio de contexto.

Si estos tres puntos estan bien resueltos, la integracion es estable, predecible y mantenible.
