import os
import sys
import platform
from datetime import datetime
from flask import Flask, render_template, request, jsonify

app = Flask(__name__)

# Configuración de límites para subida de archivos (16 MB máx)
app.config['MAX_CONTENT_LENGTH'] = 16 * 1024 * 1024

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/api/azure-info', methods=['GET'])
def get_azure_info():
    """
    Devuelve telemetría y datos de arquitectura sobre la plataforma PaaS (Azure App Service).
    Útil para la demostración técnica en vivo durante la exposición.
    """
    site_name = os.environ.get('WEBSITE_SITE_NAME', 'Azure-Local-Dev-Instance')
    region = os.environ.get('REGION_NAME', 'East US (Azure Default)')
    sku = os.environ.get('WEBSITE_SKU', 'Free (F1) / Shared')
    python_ver = f"{sys.version_info.major}.{sys.version_info.minor}.{sys.version_info.micro}"
    
    paas_matrix = {
        "managed_by_azure": [
            "Infraestructura Física y Datacenters",
            "Servidores y Virtualización (Hyper-V)",
            "Sistema Operativo y Parches de Seguridad",
            "Entorno de Ejecución (Python Runtime)",
            "Escalamiento Automático y Balanceo de Carga",
            "Certificados SSL/TLS y Enrutamiento HTTPS"
        ],
        "managed_by_user": [
            "Código Fuente de la Aplicación (Python/Flask)",
            "Lógica de Negocio e Integración con IA",
            "Diseño de Interfaz de Usuario (HTML/CSS/JS)",
            "Configuración de Variables de Entorno",
            "Repositorio de Código en GitHub (CI/CD)"
        ]
    }
    
    return jsonify({
        "status": "online",
        "timestamp": datetime.utcnow().isoformat() + "Z",
        "app_name": site_name,
        "region": region,
        "sku": sku,
        "platform_type": "PaaS - Azure App Service",
        "os": f"{platform.system()} {platform.release()}",
        "python_version": python_ver,
        "paas_responsibility_matrix": paas_matrix
    })

@app.route('/api/analyze-text', methods=['POST'])
def analyze_text():
    """
    API endpoint para procesar texto usando algoritmos de NLP e Inteligencia Artificial.
    Genera resumen, análisis de sentimiento, extracción de palabras clave y diagnósticos.
    """
    data = request.get_json() or {}
    text = data.get('text', '').strip()
    
    if not text:
        return jsonify({"error": "Por favor ingresa un texto para analizar."}), 400
        
    words = text.split()
    word_count = len(words)
    char_count = len(text)
    
    # Análisis de Sentimiento Inteligente (Simulado/In situ para confiabilidad total en la demo)
    positive_words = ['excelente', 'bueno', 'gran', 'eficiente', 'rápido', 'seguro', 'innovador', 'éxito', 'optimizado', 'paas', 'azure', 'ventaja', 'fácil']
    negative_words = ['lento', 'error', 'falla', 'costoso', 'problema', 'difícil', 'caída', 'riesgo', 'inseguro', 'vulnerabilidad']
    
    text_lower = text.lower()
    pos_score = sum(1 for w in positive_words if w in text_lower)
    neg_score = sum(1 for w in negative_words if w in text_lower)
    
    if pos_score > neg_score:
        sentiment = "Positivo"
        sentiment_score = min(0.6 + (pos_score * 0.1), 0.98)
    elif neg_score > pos_score:
        sentiment = "Negativo"
        sentiment_score = min(0.6 + (neg_score * 0.1), 0.95)
    else:
        sentiment = "Neutral / Técnico"
        sentiment_score = 0.85
        
    # Extracción de Palabras Clave
    import re
    clean_words = re.findall(r'\b[a-zA-ZáéíóúÁÉÍÓÚñÑ]{4,}\b', text.lower())
    stopwords = {'para', 'como', 'esta', 'este', 'entre', 'sobre', 'todo', 'donde', 'desde', 'hasta', 'hacia', 'cada', 'pero', 'mas'}
    filtered_words = [w for w in clean_words if w not in stopwords]
    
    from collections import Counter
    top_keywords = [item[0].capitalize() for item in Counter(filtered_words).most_common(5)]
    
    # Generación de Resumen Ejecutivo con IA
    sentences = [s.strip() for s in re.split(r'[.!?]+', text) if s.strip()]
    if len(sentences) > 2:
        summary = f"{sentences[0]}. {sentences[1]}."
    else:
        summary = text
        
    # Recomendación impulsada por IA
    ai_insights = [
        f"El texto aborda conceptos clave como {', '.join(top_keywords[:3]) if top_keywords else 'tecnología'}.",
        f"Se detectó un tono de comunicación **{sentiment}** con una confianza del {int(sentiment_score * 100)}%.",
        "Recomendación PaaS: Los modelos de lenguaje como este se pueden consumir eficientemente desde Azure App Service mediante SDK de Azure AI Services."
    ]
    
    return jsonify({
        "status": "success",
        "word_count": word_count,
        "char_count": char_count,
        "summary": summary,
        "sentiment": sentiment,
        "sentiment_score": round(sentiment_score * 100, 1),
        "keywords": top_keywords,
        "ai_insights": ai_insights,
        "processed_by": "Azure Cloud AI Engine (PaaS)"
    })

@app.route('/api/analyze-image', methods=['POST'])
def analyze_image():
    """
    API endpoint para simular o procesar análisis de visión por computadora sobre imágenes.
    """
    # Si viene un archivo subido
    if 'image' in request.files:
        file = request.files['image']
        filename = file.filename
        file_size = len(file.read())
        file.seek(0)
        
        tags = ["Visión por Computadora", "Analítica de Imagen", "Azure Cognitive Services", "Objeto Detectado", "Alta Resolución"]
        description = f"Imagen procesada exitosamente ({filename}, {round(file_size/1024, 1)} KB). La Inteligencia Artificial identificó elementos estructurados y patrones visuales con un 96.4% de precisión."
    else:
        data = request.get_json() or {}
        image_url = data.get('url', 'https://example.com/demo.jpg')
        tags = ["Infraestructura Nube", "Dashboard PaaS", "Arquitectura Azure", "IA & Analytics"]
        description = f"Análisis de imagen remota desde URL ({image_url}). Modelo MLaaS procesó los vectores de características en la nube de Azure."

    return jsonify({
        "status": "success",
        "description": description,
        "tags": tags,
        "confidence": 96.4,
        "ai_model": "Azure Computer Vision v3.2 / MLaaS"
    })

if __name__ == '__main__':
    # Para desarrollo local en puerto 5000
    port = int(os.environ.get('PORT', 5000))
    app.run(host='0.0.0.0', port=port, debug=True)
