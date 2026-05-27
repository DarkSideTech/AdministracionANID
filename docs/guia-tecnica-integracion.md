# Guia para Equipos Tecnicos de Integracion

## Objetivo

Esta guia entrega una referencia practica para equipos tecnicos que deban integrar otra aplicacion con la seguridad implementada en AUT2Services.

El foco esta en integracion, no en detalle interno de codigo.

## Principios de integracion

- la sesion viaja en cookies;
- las operaciones mutantes requieren CSRF;
- el cliente no deberia almacenar manualmente `access token` ni `refresh token` si puede usar cookies correctamente;
- la sesion completa depende de `Login` y, si aplica, `LoginOrganizacion`.

## Endpoints principales

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

## Paso a paso de integracion

### Paso 1. Habilitar cookies en el cliente HTTP

El cliente debe:

- conservar cookies entre requests;
- reenviarlas automaticamente;
- permitir lectura de la cookie `XSRF-TOKEN` para poblar el header CSRF.

### Paso 2. Resolver CSRF

Secuencia minima:

1. llamar `GET /api/account/csrf`;
2. capturar cookie `XSRF-TOKEN`;
3. para cada `POST`, `PUT`, `PATCH` o `DELETE`, enviar:
   - cookie `XSRF-TOKEN`
   - header `X-CSRF-TOKEN` con el mismo valor.

Recomendacion:

- centralizar esto en interceptor, middleware o helper compartido.

### Paso 3. Registro

Para registrar usuario:

- `POST /api/account/register`
- body: `CorreoElectronico`, `Nacionalidad`, `TipoDeUsuario`, `DocumentoDeIdentidad`, `NumeroDeDocumento`, `CodigoValidadorDocumento`, `PrimerNombre`, `SegundoNombre`, `PrimerApellido`, `SegundoApellido`, `SexoDeclarativo`, `SexoRegistral`, `FechaDeNacimiento`, `Contraseña`, `ConfirmaContraseña`, `TerminosYCondiciones`
- incluir CSRF

Resultado esperado:

- respuesta de registro;
- sin sesion autenticada;
- confirmacion de correo requerida.

### Paso 4. Confirmacion de email

Para confirmar:

- `POST /api/account/confirmemail`
- body: `UserId`, `Token`
- incluir CSRF

Recomendacion:

- implementar una pantalla o endpoint cliente que procese el enlace de confirmacion.

### Paso 5. Reenvio de confirmacion

Si el usuario no confirma:

- `POST /api/account/resendconfirmationemail`
- body: `email`
- incluir CSRF

Consideracion:

- el endpoint tiene rate limiting;
- debe manejarse `429` de forma controlada.

### Paso 6. Login

Para autenticar:

- `POST /api/account/login`
- body: `email`, `password`
- incluir CSRF

Resultado esperado:

- cookies de sesion emitidas;
- login bloqueado si `EmailConfirmed` es falso.

### Paso 7. Consultar estado actual

Despues de login o al cargar la app:

- llamar `GET /api/account/currentuser`

Usos:

- reconstruir sesion;
- saber si el usuario esta autenticado;
- saber si falta seleccion de rol;
- conocer roles disponibles.

### Paso 8. LoginOrganizacion

Si el usuario debe fijar la Organizacion operativa:

- `POST /api/account/login-2`
- body: `Organización`, con el código de la Organización seleccionada
- incluir CSRF

Resultado esperado:

- sesion operativa definitiva;
- rotacion de sesion;
- refresh token anterior invalidado.

### Paso 9. Refresh

Cuando el cliente reciba `401` por expiracion:

1. llamar `POST /api/account/refresh`;
2. incluir CSRF;
3. si funciona, reintentar request original;
4. si falla, limpiar estado local y pedir nuevo login.

Consideraciones:

- refresh conserva el rol operativo;
- refresh falla si la sesion esta invalidada;
- refresh falla si el usuario ya no tiene email confirmado.

### Paso 10. Logout

Para cerrar sesion:

- `POST /api/account/logout`
- incluir CSRF

Resultado esperado:

- cookies invalidadas;
- sesion revocada en servidor.

### Paso 11. Ejemplo practico en Angular 21 para Login

En Angular 21, la forma recomendada es resolver el CSRF antes del login y dejar que el navegador maneje las cookies de sesion.

Secuencia recomendada:

1. llamar `GET /api/accouint/csrf`;
2. leer la cookie `XSRF-TOKEN`;
3. llamar `POST /api/account/login`;
4. enviar el header `X-CSRF-TOKEN` con el mismo valor de la cookie;
5. usar `withCredentials: true` para que el navegador conserve y reenvie cookies.

Ejemplo:

```ts
import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, switchMap } from 'rxjs';

interface LoginRequest {
  email: string;
  password: string;
}

interface AuthResponse {
  expiresAtUtc: string;
  selectedRole: string | null;
  roleSelectionRequired: boolean;
  user: {
    id: string;
    email: string;
    roles: string[];
  };
}

@Injectable({ providedIn: 'root' })
export class AuthIntegrationService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5219/api/account';

  login(payload: LoginRequest): Observable<AuthResponse> {
    return this.http.get(`${this.baseUrl}/csrf`, {
      withCredentials: true,
      observe: 'response'
    }).pipe(
      switchMap(() => {
        const xsrfToken = this.readCookie('XSRF-TOKEN');

        return this.http.post<AuthResponse>(`${this.baseUrl}/login`, payload, {
          withCredentials: true,
          headers: {
            'X-CSRF-TOKEN': xsrfToken ?? ''
          }
        });
      })
    );
  }

  private readCookie(name: string): string | null {
    const encodedName = `${name}=`;
    const cookies = document.cookie.split(';');

    for (const cookie of cookies) {
      const value = cookie.trim();
      if (value.startsWith(encodedName)) {
        return decodeURIComponent(value.substring(encodedName.length));
      }
    }

    return null;
  }
}
```

Puntos importantes del ejemplo:

- `GET /csrf` debe ejecutarse antes del login;
- `withCredentials: true` permite que el navegador reciba y reenvie cookies;
- la cookie `XSRF-TOKEN` es legible desde JavaScript porque no es `HttpOnly`;
- el header `X-CSRF-TOKEN` debe llevar exactamente el mismo valor que la cookie `XSRF-TOKEN`;
- las cookies de autenticacion emitidas por login no deben copiarse manualmente.

Recomendacion:

- en una aplicacion Angular real, esta logica conviene moverla a un servicio de CSRF o a un interceptor para no repetirla en cada metodo.

## Recomendaciones tecnicas

### Para aplicaciones modernas

- usar cookie jar del navegador o runtime;
- resolver CSRF automaticamente;
- usar `currentuser` como fuente de estado;
- implementar refresh en un punto central.

### Para sistemas legados

- encapsular autenticacion en un modulo unico;
- capturar cookies manualmente si hace falta;
- no repartir logica de CSRF en muchas pantallas;
- separar `Login`, `LoginOrganizacion` y `RefreshToken` en pasos simples y auditables.

## Errores frecuentes

- olvidar pedir `/api/account/csrf` antes de un `POST`;
- enviar `X-CSRF-TOKEN` distinto a la cookie `XSRF-TOKEN`;
- no conservar cookies entre requests;
- asumir que `register` deja al usuario logueado;
- intentar saltarse `LoginOrganizacion` cuando hay seleccion de rol;
- no contemplar `401` en refresh o `429` en resend-confirmation-email.

## Resumen

La integracion correcta depende de tres capacidades del cliente:

1. manejo de cookies;
2. manejo correcto de CSRF;
3. soporte para sesion en dos etapas cuando el rol operativo debe fijarse.

Si esos tres puntos se resuelven bien, la integracion con esta propuesta es estable y predecible.
