# PersonAPI-DotNet

API REST desarrollada en .NET 8 con SQL Server 2019 Express para la gestión de personas, profesiones, teléfonos y estudios. Utiliza el patrón MVC y DAO. Documentada con Swagger y ejecutable mediante Docker.

## 🚀 Requisitos

Antes de clonar y ejecutar el proyecto, asegúrate de tener instalado lo siguiente:
  
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)  
- [Git](https://git-scm.com/)  

---

## 🧾 Clonar el repositorio

```bash
git clone https://github.com/tuusuario/personapi-dotnet.git
cd personapi-dotnet


# PersonAPI-DotNet
API REST desarrollada en .NET 8 con SQL Server 2019 Express para la gestión de personas, profesiones, teléfonos y estudios. Utiliza el patrón MVC y DAO. Documentada con Swagger y ejecutable mediante Docker.
## 🚀 Requisitos
Antes de clonar y ejecutar el proyecto, asegúrate de tener instalado lo siguiente:
  
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)  
- [Git](https://git-scm.com/)  
---
## 🧾 Clonar el repositorio
```bash
git clone https://github.com/tuusuario/personapi-dotnet.git
cd personapi-dotnet
```

## 🐳 Ejecutar con Docker Compose
El proyecto incluye configuración Docker Compose para facilitar su despliegue. Sigue estos pasos:

### 1. Construir y levantar los contenedores
```bash
docker-compose up -d --build
```

Este comando:
- Construye las imágenes necesarias
- Crea los contenedores para la API y SQL Server
- Inicia los servicios en modo detached (segundo plano)

### 2. Verificar que los contenedores están en ejecución
```bash
docker-compose ps
```
