document.addEventListener('DOMContentLoaded', () => {
    
    // Initial fetch of Azure PaaS Telemetry
    fetchAzureTelemetry();

    // 1. TAB NAVIGATION
    const navButtons = document.querySelectorAll('.nav-btn');
    const tabContents = document.querySelectorAll('.tab-content');

    navButtons.forEach(btn => {
        btn.addEventListener('click', () => {
            const targetTab = btn.getAttribute('data-tab');

            navButtons.forEach(b => b.classList.remove('active'));
            tabContents.forEach(t => t.classList.remove('active'));

            btn.classList.add('active');
            document.getElementById(targetTab).classList.add('active');
        });
    });

    // 2. TEXT ANALYTICS
    const btnAnalyzeText = document.getElementById('btn-analyze-text');
    const btnSampleText = document.getElementById('btn-sample-text');
    const inputText = document.getElementById('input-text');
    const textResultsBody = document.getElementById('text-results-body');

    btnSampleText.addEventListener('click', () => {
        inputText.value = "Plataforma como Servicio (PaaS) es un entorno de desarrollo y despliegue completo en la nube. Con soluciones como Azure App Service, AWS Elastic Beanstalk o Vercel, los desarrolladores pueden enfocarse únicamente en construir su software e integrar modelos de Inteligencia Artificial mediante API/SDK. El proveedor de la nube gestiona la seguridad del servidor, parches del sistema operativo y escalado automático de forma excelente e innovadora.";
    });

    btnAnalyzeText.addEventListener('click', async () => {
        const text = inputText.value.trim();
        if (!text) {
            alert('Por favor ingresa o carga un texto para analizar.');
            return;
        }

        textResultsBody.innerHTML = `
            <div class="placeholder-msg">
                <i class="fa-solid fa-spinner fa-spin" style="color: var(--azure-cyan);"></i>
                <p>Procesando texto mediante Azure Cloud AI Engine...</p>
            </div>
        `;

        try {
            const response = await fetch('/api/analyze-text', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ text: text })
            });

            const data = await response.json();

            if (response.ok) {
                renderTextResults(data);
            } else {
                textResultsBody.innerHTML = `<div class="alert alert-danger">${data.error || 'Error procesando texto.'}</div>`;
            }
        } catch (err) {
            console.error(err);
            textResultsBody.innerHTML = `<div class="alert alert-danger">Error de conexión con el servidor.</div>`;
        }
    });

    function renderTextResults(data) {
        let sentimentClass = data.sentiment === 'Positivo' ? 'text-green' : 'text-blue';
        
        let keywordsHtml = data.keywords.map(kw => `<span class="stat-badge"><i class="fa-solid fa-tag"></i> ${kw}</span>`).join('');
        let insightsHtml = data.ai_insights.map(ins => `<li><i class="fa-solid fa-check-circle text-blue"></i> ${ins}</li>`).join('');

        textResultsBody.innerHTML = `
            <div class="result-box">
                <h4><i class="fa-solid fa-bullseye"></i> Resumen Ejecutivo</h4>
                <p class="mt-2" style="font-size: 0.95rem; line-height: 1.5; color: #e5e7eb;">${data.summary}</p>
            </div>

            <div class="grid-2col mt-3">
                <div class="result-box">
                    <span class="telemetry-item">Sentimiento Detectado:</span><br>
                    <strong class="${sentimentClass}" style="font-size: 1.1rem;">${data.sentiment} (${data.sentiment_score}%)</strong>
                </div>
                <div class="result-box">
                    <span class="telemetry-item">Métricas del Documento:</span><br>
                    <strong>${data.word_count} Palabras / ${data.char_count} Caracteres</strong>
                </div>
            </div>

            <div class="mt-3">
                <label>Palabras Clave Identificadas:</label>
                <div>${keywordsHtml}</div>
            </div>

            <div class="result-box mt-3">
                <h4><i class="fa-solid fa-lightbulb"></i> Diagnóstico de la Nube (PaaS)</h4>
                <ul class="insight-list">${insightsHtml}</ul>
            </div>
        `;
    }

    // 3. VISION ANALYTICS / DROP ZONE
    const dropZone = document.getElementById('drop-zone');
    const imageFileInput = document.getElementById('image-file-input');
    const visionResults = document.getElementById('vision-results');
    const visionDescription = document.getElementById('vision-description');
    const visionTags = document.getElementById('vision-tags');

    dropZone.addEventListener('dragover', (e) => {
        e.preventDefault();
        dropZone.style.borderColor = 'var(--azure-cyan)';
    });

    dropZone.addEventListener('dragleave', () => {
        dropZone.style.borderColor = 'rgba(80, 230, 255, 0.3)';
    });

    dropZone.addEventListener('drop', (e) => {
        e.preventDefault();
        dropZone.style.borderColor = 'rgba(80, 230, 255, 0.3)';
        if (e.dataTransfer.files.length > 0) {
            handleImageUpload(e.dataTransfer.files[0]);
        }
    });

    imageFileInput.addEventListener('change', (e) => {
        if (e.target.files.length > 0) {
            handleImageUpload(e.target.files[0]);
        }
    });

    async function handleImageUpload(file) {
        const formData = new FormData();
        formData.append('image', file);

        visionResults.classList.remove('hidden');
        visionDescription.innerText = 'Enviando imagen a Azure Computer Vision MLaaS...';
        visionTags.innerHTML = '';

        try {
            const response = await fetch('/api/analyze-image', {
                method: 'POST',
                body: formData
            });

            const data = await response.json();
            if (response.ok) {
                visionDescription.innerText = data.description;
                visionTags.innerHTML = data.tags.map(t => `<span class="stat-badge">${t}</span>`).join('');
            }
        } catch (err) {
            console.error(err);
            visionDescription.innerText = 'Error al procesar la imagen.';
        }
    }

    // 4. FETCH AZURE TELEMETRY
    async function fetchAzureTelemetry() {
        const liveStatusText = document.getElementById('azure-status-text');

        try {
            const res = await fetch('/api/azure-info');
            const data = await res.json();

            if (res.ok) {
                liveStatusText.innerText = `${data.platform_type} (${data.app_name})`;

                document.getElementById('t-appname').innerText = data.app_name;
                document.getElementById('t-region').innerText = data.region;
                document.getElementById('t-python').innerText = `Python ${data.python_version} (Flask + Gunicorn)`;
                document.getElementById('t-sku').innerText = data.sku;
                document.getElementById('t-os').innerText = data.os;

                // Render responsabilidad
                if (data.paas_responsibility_matrix) {
                    const azureUl = document.getElementById('list-azure-managed');
                    const userUl = document.getElementById('list-user-managed');

                    if (azureUl && data.paas_responsibility_matrix.managed_by_azure) {
                        azureUl.innerHTML = data.paas_responsibility_matrix.managed_by_azure
                            .map(item => `<li><i class="fa-solid fa-check text-green"></i> ${item}</li>`).join('');
                    }

                    if (userUl && data.paas_responsibility_matrix.managed_by_user) {
                        userUl.innerHTML = data.paas_responsibility_matrix.managed_by_user
                            .map(item => `<li><i class="fa-solid fa-code text-blue"></i> ${item}</li>`).join('');
                    }
                }
            }
        } catch (err) {
            console.error(err);
            liveStatusText.innerText = 'Servidor Local (Listo para desplegar a Azure App Service)';
        }
    }
});
