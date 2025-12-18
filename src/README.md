[TOC]

------

# **Deploy en Ubuntu** 

Este documento explica como deployar e instalar los servicios en Ubuntu, en este caso es utilizaron los siguientes elementos:

- Visual Studio 2022
- Windows 11
- WSL2-Ubuntu 22.04



# Publicar el servicio 

A continuación se detallas las acciones necesarias para la generación de los archivos binarios del servicio

## Mediante línea de comandos - Framework-Dependant

Para publicar un servicio como `Framework-dependant` se puede realizar el siguiente comando, el cual genera los archivos binarios necesarios sin incluir gran parte de las librerías .net. 

```powershell
dotnet publish --configuration Release
```



## Mediante línea de comandos - Self-Contained

Para publicar un servicio como `Self-Contained` se puede realizar el siguiente comando, el cual genera los archivos binarios que incluyen todas la librerías que se utilizan en el desarrollo, incluyendo la requeridas de .net. 

```powershell
dotnet publish -c Release -o /app/publish -r linux-x64 --self-contained true
```



# Instalación servicio

## Directorio de Instalación

Para la instalación se definirá un directorio con el nombre del dominio en Ubuntu

```bash
/servicios/aut2services
```

Copie el directorio generado que contiene la aplicación ASP.NET Core al servidor mediante una herramienta que se integre en el flujo de trabajo de la organización (por ejemplo, SCP, SFTP).



## Crear el monitor del Servicio

Se crea un archivo de inicio para el servicio, el cual sirve para monitorear el estado y la información entregada por el servicio

Crear el archivo con el detalle del monitor para el servicio

```bash
sudo nano /etc/systemd/system/aut2services.service
```

Texto que debe tener el archivo transitos.service

```
[Unit] 
Description=Este Servicio contiene las API's para em naejo de la Autenticacion y Autorizacion para los sistemas ANID

[Service] 
WorkingDirectory=/servicios/aut2services
ExecStart=/usr/bin/dotnet /servicios/aut2services/AUT2Services.Services.API.dll 
Restart=always 

# Restart service after 10 seconds if the dotnet service crashes: 
RestartSec=10 
KillSignal=SIGINT 
SyslogIdentifier=dotnet-kestrel-service

# Variables de entorno para la ejecucion del servicio
Environment=ASPNETCORE_ENVIRONMENT=Development
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false 

[Install] 
WantedBy=multi-user.target
```

Habilitar el servicio

```bash
sudo systemctl enable aut2services.service
```

Iniciar el servicio

```bash
sudo systemctl start aut2services.service
```

Verificar el estado del servicio

```bash
sudo systemctl status aut2services.service
```

Revisar los logs de proceso

```bash
sudo journalctl -fu aut2services.service
```



# Agregar el repositorio de paquetes de Microsoft (Opcional)

El repositorio de paquetes de Microsoft, permite mantener una actualización de las librerías que utiliza .net

```
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
```



# Instalar .NET SDK o .NET Runtime en Ubuntu

[Link Oficial](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)

Instalar SDK 

```
sudo apt-get update && \  
  sudo apt-get install -y dotnet-sdk-9.0
```

Instalar Runtime - Para el caso de una implementación `Self-Contained` solo es necesario el runtime

```bash
sudo apt-get update && \  
  sudo apt-get install -y aspnetcore-runtime-9.0
```



# Como utilizar ASP.NET Core con Nginx (Opcional)

No es estrictamente necesario usar Nginx, pero es **altamente recomendado** para publicar una API de ASP.NET Core autocontenida en Ubuntu. Nginx actúa como un **proxy inverso**, lo que permite mejorar el rendimiento, la seguridad y la escalabilidad al gestionar solicitudes, distribuir la carga y servir contenido estático.

Prerrequisitos

- Ubuntu 20.04 o superior con un usuario standard con privilegios sudo
- La ultima y estable [.NET runtime Instalado](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu) en el servidor

## Instalar Nginx en Ubuntu

Con los siguientes pasos se puede instalar Nginx en Ubuntu

```bash
sudo apt update
sudo apt install nginx
```

[Nginx: Official Debian/Ubuntu packages](https://www.nginx.com/resources/wiki/start/topics/tutorials/install/#official-debian-ubuntu-packages)

Ejecutar servicio

```bash
sudo service nginx start
```

Detener servicio

```bash
sudo service nginx stop
```

## Configurar Nginx en Ubuntu

**Configura Nginx**: Edita el archivo de configuración de Nginx para tu sitio (por ejemplo, `/etc/nginx/sites-available/your_app`) 

```bash
server {
    listen 80;
    server_name your_domain.com;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```



## Verificar configuracion Nginx en Ubuntu

Verifica que Nginx y la aplicación estén corriendo correctamente.

```bash
sudo systemctl status nginx
sudo systemctl status aut2services.service
```



# Configurar en Ubuntu SSH

Instale **SSH** en su máquina [WSL](https://es.a-d.site/?cat=linux) Ubuntu y asegúrese de habilitarlo.

```bash
sudo apt remove openssh-server

sudo apt install openssh-server 
```

Iniciar servidor SSH

```bash
 sudo service ssh start
```

Detener servidor SSH

```bash
 sudo service ssh stop
```

Ver estado servidor SSH

```bash
 sudo service ssh status
```

Con SSH funcionando, verifique su dirección IP y debe instalar las **herramientas de red** para usar el comando **ifconfig.**

```bash
sudo apt install net-tools

ifconfig 
```

