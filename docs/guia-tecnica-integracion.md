# Guía técnica de integración con AUT2Services

## Alcance

Esta guía define el contrato técnico del flujo de autenticación convencional de AUT2Services. Está dirigida a equipos que integran aplicaciones web, clientes HTTP o sistemas legados con la API.

El alcance incluye cookies, CSRF, login, selección de organización, refresh, logout y las pruebas de apoyo en Postman. No incorpora validaciones de mecanismos de autenticación fuera de este flujo convencional.

## Endpoints de sesión

| Método | Ruta | Uso |
|---|---|---|
| `GET` | `/healthz` | Verificación anónima de disponibilidad. |
| `GET` | `/api/account/csrf` | Emite o conserva la cookie CSRF. |
| `POST` | `/api/account/register` | Registra un usuario. |
| `POST` | `/api/account/confirmemail` | Confirma el correo del usuario. |
| `POST` | `/api/account/resendconfirmationemail` | Reenvía la confirmación, sujeto a límites. |
| `POST` | `/api/account/login` | Inicia la sesión convencional. |
| `GET` | `/api/account/currentuser` | Recupera el usuario y el estado actual. |
| `GET` | `/api/account/miinformacion` | Recupera información del usuario autenticado. |
| `POST` | `/api/account/loginorganizacion` | Completa el contexto operativo seleccionado. |
| `POST` | `/api/account/refreshtoken` | Rota la sesión mediante la cookie de refresh. |
| `POST` | `/api/account/logout` | Revoca la sesión y elimina las cookies de autenticación. |

## Cookies y CSRF

La API utiliza las siguientes cookies:

| Cookie | Cliente puede leerla | Propósito |
|---|---:|---|
| `XSRF-TOKEN` | Sí | Debe copiarse en el encabezado `X-CSRF-TOKEN`. |
| `accesstoken` | No | Token de acceso en cookie `HttpOnly`. |
| `refreshToken` | No | Token de renovación en cookie `HttpOnly`. |

La secuencia requerida para una operación mutante es:

1. ejecutar `GET /api/account/csrf`;
2. conservar la cookie `XSRF-TOKEN`;
3. enviar esa misma cadena en `X-CSRF-TOKEN`;
4. conservar todas las cookies de respuesta para las llamadas posteriores.

Si falta la cookie o el encabezado no coincide, el backend rechaza la operación. El cliente debe centralizar esta conducta en un interceptor, middleware o helper común.

## Login y contexto operativo

### Login

```text
POST /api/account/login
Content-Type: application/json
X-CSRF-TOKEN: [valor de la cookie XSRF-TOKEN]
```

El cuerpo contiene las credenciales convencionales del usuario. La respuesta emite las cookies de sesión y describe si se requiere completar el contexto de organización.

### Selección de organización

Cuando la sesión lo requiera:

```text
POST /api/account/loginorganizacion
Authorization: Bearer [access token de la sesión]
X-CSRF-TOKEN: [valor de la cookie XSRF-TOKEN]

{
  "organizacion": "[CODIGO_ORGANIZACION]"
}
```

La API valida que la organización esté disponible para el usuario y rota las credenciales de sesión. No se debe asumir que el login inicial habilita todas las operaciones protegidas.

## Estado de sesión, refresh y logout

Use `GET /api/account/currentuser` para reconstruir el estado al cargar una aplicación y para determinar si falta selección de organización.

Ante un `401` causado por expiración de sesión:

1. ejecute una única llamada a `POST /api/account/refreshtoken` con CSRF;
2. si tiene éxito, reintente la solicitud original una sola vez;
3. si falla, elimine el estado visual local y dirija al usuario a login.

El refresh token se mantiene en cookie, se persiste como hash, se rota en cada uso y puede revocarse ante expiración, cierre de sesión, cambios sensibles o detección de reuso.

Para cerrar sesión, invoque `POST /api/account/logout` con CSRF. El backend revoca la sesión aplicable y limpia las cookies de autenticación.

## Uso de Postman

La colección y el ambiente local están en `docs/postman/`.

| Archivo | Uso |
|---|---|
| `AUT2Services.local.postman_collection.json` | Flujo convencional, CSRF y APIs operativas seleccionadas. |
| `AUT2Services.local.postman_environment.json` | Variables del ambiente local. |

Configure `baseUrl` con la dirección del backend en ejecución. El ambiente distribuido está orientado al perfil local, cuyo puerto configurado es `5002`.

Ejecute **CSRF Bootstrap** antes de **Login**. Las validaciones ya presentes en la colección conservan `csrfToken`, `accessToken`, cookies y organización seleccionada; no las elimine ni reemplace por valores fijos. La colección no debe contener secretos, cookies o tokens reutilizables.

Para un ambiente distinto, cree un ambiente local privado y cambie solamente `baseUrl` y las credenciales de prueba necesarias. No modifique ni publique la colección compartida para almacenar secretos.

## Ejemplo Angular

En Angular, el cliente debe usar `withCredentials: true` y obtener CSRF antes del login.

```ts
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthIntegrationService {
  private readonly http = inject(HttpClient);
  private readonly accountApi = 'http://localhost:5002/api/account';

  login(email: string, password: string) {
    return this.http.get(`${this.accountApi}/csrf`, {
      withCredentials: true,
      observe: 'response'
    }).pipe(
      switchMap(() => this.http.post(`${this.accountApi}/login`, { email, password }, {
        withCredentials: true,
        headers: { 'X-CSRF-TOKEN': this.readCookie('XSRF-TOKEN') ?? '' }
      }))
    );
  }

  private readCookie(name: string): string | null {
    const prefix = `${name}=`;
    const value = document.cookie.split(';').map(item => item.trim()).find(item => item.startsWith(prefix));
    return value ? decodeURIComponent(value.substring(prefix.length)) : null;
  }
}
```

En un despliegue con frontend y backend bajo el mismo dominio, se recomienda usar una ruta relativa para la API, por ejemplo `/api/account`, y mantener el manejo de cookies en el navegador.

## Diagnóstico técnico

| Síntoma | Verificación inicial |
|---|---|
| `400` o error CSRF | Confirmar cookie y encabezado `X-CSRF-TOKEN` con el mismo valor. |
| `401` después de login | Consultar `currentuser`, revisar selección de organización e intentar un refresh una sola vez. |
| `429` en reenvío o recuperación | Respetar el tiempo de espera; no realizar reintentos automáticos en bucle. |
| Sin cookies en Postman o navegador | Revisar cookie jar, `withCredentials`, HTTPS y configuración del dominio. |
| API inaccesible | Verificar primero `GET /healthz`. |

## Criterios de seguridad

- No registrar contraseñas, cookies, tokens, valores CSRF ni encabezados de autorización.
- No mover tokens de sesión a almacenamiento web persistente.
- Mantener HTTPS y cookies seguras en ambientes publicados.
- Aplicar el principio de un único refresh por solicitud fallida.
- Usar las migraciones como fuente de verdad de la persistencia y datos de prueba solo en entornos autorizados.
