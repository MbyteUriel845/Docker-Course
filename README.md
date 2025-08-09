# Docker-Course

docker-compose up --build

# 📦 Curso de Docker – Guía Esencial

Este repositorio es parte de un curso de introducción a **Docker**, donde aprenderás los conceptos básicos para crear imágenes, levantar contenedores y administrarlos de forma sencilla.

---

## 🚀 ¿Qué es Docker?
Docker es una plataforma que permite **empaquetar aplicaciones y sus dependencias** en un contenedor, asegurando que se ejecuten de forma idéntica en cualquier entorno.

---

## 🛠 Requisitos previos
- Tener instalado [Docker Desktop](https://www.docker.com/products/docker-desktop/) o Docker Engine.

---

## 📂 Conceptos básicos

### 🔹 Imagen
Es una **plantilla inmutable** que contiene todo lo necesario para ejecutar una aplicación.

### 🔹 Contenedor
Es una **instancia en ejecución de una imagen**. Piensa en él como un “proceso” aislado.

---

## 📌 Comandos esenciales

### 1️⃣ Verificar instalación
docker --version
docker info

### 2️⃣ Descargar una imagen existente
docker pull nginx:latest

### 3️⃣ Listar imágenes locales
docker images

### 4️⃣ Crear y ejecutar un contenedor
docker run -d --name mi-nginx -p 8080:80 nginx:latest

-d → modo “detached” (en segundo plano).
--name → nombre personalizado para el contenedor.
-p host:container → mapea el puerto 8080 local al puerto 80 del contenedor.

### 5️⃣ Ver contenedores en ejecución
docker ps

### 6️⃣ Ver todos los contenedores (incluso detenidos)
docker ps -a

### 7️⃣ Detener y eliminar contenedores
docker stop mi-nginx
docker rm mi-nginx

### 8️⃣ Eliminar imágenes
docker rmi nginx:latest

