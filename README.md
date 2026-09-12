# 🚀 Azure AI Cloud Hub - Proyecto PaaS (Plataforma como Servicio)

Este proyecto es una **Aplicación Web e Integración con Inteligencia Artificial** desarrollada para la materia de Ingeniería Informática (Tecnológico Superior de Jalisco / ITMM), orientada a la demostración práctica del modelo **Plataforma como Servicio (PaaS)** utilizando **Azure App Service**.

---

## 📋 Características Principales del Proyecto

1. **Análisis de Texto e Inteligencia de Contenido:** 
   * Procesa texto para generar resúmenes ejecutivos, detectar el sentimiento (Positivo/Neutral/Negativo), extraer palabras clave y ofrecer diagnósticos impulsados por IA.
2. **Reconocimiento y Visión por Computadora (MLaaS):**
   * Permite subir imágenes para procesarlas en la nube de Azure mediante algoritmos de visión artificial.
3. **Panel de Telemetría y Matriz de Responsabilidad Compartida:**
   * Muestra en tiempo real las variables de entorno de la instancia PaaS en Azure (Región, Runtime Python, SKU/Plan, SO Host) y la comparación de responsabilidades entre el proveedor (CSP) y el cliente.

---

## 🛠️ Cómo Probar la Aplicación en Local (Tu Computadora)

1. Abre una terminal en este directorio y asegúrate de tener Python instalado.
2. (Opcional) Crea y activa un entorno virtual:
   ```bash
   python -m venv venv
   # En Windows:
   venv\Scripts\activate
   ```
3. Instala las dependencias:
   ```bash
   pip install -r requirements.txt
   ```
4. Ejecuta el servidor Flask:
   ```bash
   python app.py
   ```
5. Abre en tu navegador: `http://localhost:5000`

---

## ☁️ Pasos para Desplegar en Azure App Service (PaaS) desde GitHub

### Paso 1: Subir tu Código a GitHub
1. Inicializa el repositorio Git en la carpeta de este proyecto:
   ```bash
   git init
   git add .
   git commit -m "Primer commit - Proyecto PaaS Azure AI Hub"
   ```
2. Crea un repositorio en tu cuenta de [GitHub](https://github.com/new) llamado `proyecto-azure-paas`.
3. Sube tu proyecto a GitHub:
   ```bash
   git remote add origin https://github.com/TU_USUARIO/proyecto-azure-paas.git
   git branch -M main
   git push -u origin main
   ```

### Paso 2: Configurar el Servicio PaaS en Azure Portal
1. Entra a [https://portal.azure.com](https://portal.azure.com) e inicia sesión.
2. En la barra superior busca **App Services** (Servicios de aplicaciones) y haz clic en **+ Crear** -> **Web App** (Aplicación web).
3. Configura los datos básicos:
   * **Suscripción:** Tu suscripción de Azure.
   * **Grupo de recursos:** Crear nuevo -> `Grupo-Proyecto-PaaS`.
   * **Nombre de la App:** `mi-proyecto-paas-ai` (debe ser un nombre único).
   * **Publicar:** Código.
   * **Pila de runtime (Runtime stack):** Python 3.10 o Python 3.11.
   * **Sistema operativo:** Linux.
   * **Región:** East US (o la más cercana).
   * **Plan de precios:** Selecciona **Free F1 (Gratuito)** o el plan básico.
4. Ve a la pestaña **Despliegue (Deployment)**:
   * Habilita el **Despliegue continuo**.
   * Selecciona **GitHub** como origen y vincula tu cuenta.
   * Selecciona el repositorio `proyecto-azure-paas` y la rama `main`.
5. Haz clic en **Revisar y crear** -> **Crear**.

### Paso 3: Probar en Vivo en la Exposición
Una vez completado el despliegue (tarda entre 2 y 4 minutos), Azure te proporcionará la URL pública (ejemplo: `https://mi-proyecto-paas-ai.azurewebsites.net`).
¡Abre esa URL durante tu exposición para demostrar la aplicación funcionando en vivo desde la nube!
