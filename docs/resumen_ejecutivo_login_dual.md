# Resumen ejecutivo: acceso con correo electrónico o nombre de usuario

## Objetivo

Se propone ajustar el inicio de sesión del sistema para permitir que una persona usuaria pueda autenticarse utilizando indistintamente su correo electrónico o su nombre de usuario, junto con su clave de acceso.

Este cambio busca mejorar la flexibilidad del acceso sin alterar el comportamiento funcional de ClaveÚnica ni reducir los controles de seguridad existentes.

## Alcance del cambio

El cambio aplica al mecanismo de inicio de sesión con credenciales propias del sistema, es decir, usuario/correo más clave de acceso.

El sistema deberá aceptar en el mismo campo de login cualquiera de estos datos:

- Correo electrónico.
- Nombre de usuario.

El mensaje visible sugerido para el campo de acceso es:

> Ingrese nombre de usuario o correo electrónico

## Comportamiento esperado

Cuando una persona ingrese sus credenciales, el sistema realizará la validación de la siguiente forma:

1. Buscará primero si existe una cuenta asociada al correo electrónico ingresado.
2. Si no encuentra una cuenta por correo electrónico, buscará una cuenta asociada al nombre de usuario ingresado.
3. Si encuentra una cuenta, validará la clave de acceso contra esa única cuenta.
4. Si la clave es correcta y la cuenta cumple las condiciones normales de acceso, permitirá el inicio de sesión.
5. Si no encuentra cuenta o la clave no corresponde, mostrará un mensaje único de error.

El sistema no intentará validar la misma clave contra múltiples cuentas. Esto evita ambigüedades y reduce riesgos de seguridad.

## Reglas de unicidad

Para que el login dual sea seguro y claro, se aceptan las siguientes reglas:

- El correo electrónico debe ser único.
- El nombre de usuario debe ser único.
- Un correo electrónico no podrá ser usado como nombre de usuario.
- Un nombre de usuario no podrá coincidir con el correo electrónico de otra cuenta.
- No se permitirá que un nombre de usuario tenga formato de correo electrónico.

Estas reglas deben aplicarse al registrar nuevas cuentas y también cuando se modifiquen datos de cuentas existentes.

## Consideraciones de seguridad aceptadas

Se incorporan las siguientes recomendaciones de seguridad:

### Mensaje único de error

El sistema no debe informar si falló porque el correo no existe, el nombre de usuario no existe o la clave es incorrecta.

Se debe usar un mensaje genérico, por ejemplo:

> Usuario o clave inválidos.

Esto evita entregar información útil a terceros que intenten descubrir cuentas registradas.

### Validación contra una sola cuenta

Si el sistema encuentra una cuenta por correo electrónico, validará la clave solo contra esa cuenta.

Solo si no existe una cuenta con ese correo, buscará por nombre de usuario.

Esto evita que una misma entrada pueda probarse contra más de una cuenta.

### Normalización de datos

Antes de comparar correos y nombres de usuario, el sistema debe normalizarlos internamente. Esto evita duplicidades por diferencias como mayúsculas, minúsculas o caracteres equivalentes.

Ejemplo de problema que se busca evitar:

- `Usuario@Test.com`
- `usuario@test.com`

Ambos deben considerarse el mismo correo electrónico.

### Control de caracteres en nombre de usuario

El nombre de usuario debe tener una política clara de caracteres permitidos.

Se recomienda evitar:

- caracteres invisibles;
- símbolos confusos;
- espacios innecesarios;
- caracteres Unicode visualmente parecidos a letras comunes;
- nombres con formato de correo electrónico.

Esto reduce riesgos de suplantación visual o confusión operativa.

### Validación obligatoria en backend

Las validaciones de seguridad no deben depender solo del frontend.

Aunque la interfaz pueda ayudar al usuario con mensajes preventivos, la decisión final debe realizarse siempre en el backend.

## Impacto esperado

### Frontend

Se requiere ajustar la pantalla de login para que el campo ya no sea tratado únicamente como correo electrónico.

Cambios esperados:

- Cambiar el mensaje del campo a “Ingrese nombre de usuario o correo electrónico”.
- Quitar validación que obligue a formato de correo en ese campo.
- Enviar el valor como identificador de acceso, no exclusivamente como correo.

### Backend

Se requiere ajustar la validación de acceso para:

- buscar primero por correo electrónico;
- buscar por nombre de usuario solo si no existe correo;
- validar la clave contra una sola cuenta;
- aplicar normalización interna;
- validar unicidad de correo y nombre de usuario;
- impedir cruces entre correos y nombres de usuario;
- rechazar nombres de usuario con formato de correo electrónico.

### Base de datos

No se anticipa la necesidad de crear una nueva columna para el nombre de usuario, ya que el sistema cuenta con estructura estándar para almacenar correo electrónico y nombre de usuario.

Sin embargo, se recomienda revisar si la unicidad del correo electrónico está garantizada también a nivel de base de datos. Si no lo está, podría ser necesario incorporar un índice único para reforzar la integridad de datos.

## Riesgos controlados

La propuesta controla los principales riesgos asociados al login dual:

- enumeración de cuentas;
- uso ambiguo de correo y nombre de usuario;
- duplicidad por mayúsculas/minúsculas;
- uso de nombres de usuario con apariencia engañosa;
- validación de clave contra más de una cuenta;
- dependencia excesiva de validaciones de frontend.

## Pendientes antes de implementar

Antes de aplicar cambios, se recomienda confirmar:

1. Texto final exacto para el campo de login.
2. Política final de caracteres permitidos para nombre de usuario.
3. Si se reforzará la unicidad del correo electrónico también en base de datos.
4. Si existen datos actuales duplicados o inconsistentes que deban limpiarse antes del cambio.
5. Mensaje final de error genérico para credenciales inválidas.

## Estado

Este documento resume la propuesta funcional y de seguridad aceptada para evaluación por TI.

No se han realizado cambios en los repositorios ni en la base de código.

La implementación debe ejecutarse solo después de confirmación explícita.
