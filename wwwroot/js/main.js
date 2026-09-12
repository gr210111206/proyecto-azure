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

    // 2. CREATE TICKET & TRIAJE INTELIGENTE
    const btnCreateTicket = document.getElementById('btn-create-ticket');
    const btnSampleTicket = document.getElementById('btn-sample-ticket');
    const ticketTitle = document.getElementById('ticket-title');
    const ticketDesc = document.getElementById('ticket-desc');
    const ticketResultsBody = document.getElementById('ticket-results-body');

    btnSampleTicket.addEventListener('click', () => {
        ticketTitle.value = "Falla Crítica: Caída del Servidor de Base de Datos SQL";
        ticketDesc.value = "Los usuarios no pueden ingresar al sistema. Marca error HTTP 500 y las transacciones de la base de datos están completamente bloqueadas en producción.";
    });

    btnCreateTicket.addEventListener('click', async () => {
        const title = ticketTitle.value.trim();
        const desc = ticketDesc.value.trim();

        if (!title && !desc) {
            alert('Por favor ingresa el título o la descripción del reporte de falla.');
            return;
        }

        ticketResultsBody.innerHTML = `
            <div class="placeholder-msg">
                <i class="fa-solid fa-spinner fa-spin" style="color: var(--azure-cyan);"></i>
                <p>Enviando reporte a la API de Azure y clasificando ticket con IA...</p>
            </div>
        `;

        try {
            const response = await fetch('/api/tickets/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ title: title, description: desc })
            });

            const data = await response.json();

            if (response.ok) {
                renderTicketResults(data);
            } else {
                ticketResultsBody.innerHTML = `<div class="alert alert-danger">${data.error || 'Error creando ticket.'}</div>`;
            }
        } catch (err) {
            console.error(err);
            ticketResultsBody.innerHTML = `<div class="alert alert-danger">Error de conexión con la API en Azure.</div>`;
        }
    });

    function renderTicketResults(data) {
        let badgeColor = data.priority_color === 'danger' ? '#ef4444' : (data.priority_color === 'warning' ? '#f59e0b' : '#10b981');

        ticketResultsBody.innerHTML = `
            <div class="result-box" style="border-left: 4px solid ${badgeColor};">
                <div style="display: flex; justify-content: space-between; align-items: center;">
                    <span class="stat-badge"><i class="fa-solid fa-hashtag"></i> ${data.ticket_id}</span>
                    <span style="font-size: 0.8rem; color: #9ca3af;"><i class="fa-regular fa-clock"></i> ${data.timestamp}</span>
                </div>
                <h4 class="mt-2" style="font-size: 1.1rem; color: #ffffff;">${data.title}</h4>
                <p style="font-size: 0.9rem; color: #9ca3af; margin-top: 0.3rem;">${data.description}</p>
            </div>

            <div class="grid-2col mt-3">
                <div class="result-box">
                    <span class="telemetry-item">Prioridad Asignada por IA:</span><br>
                    <strong style="color: ${badgeColor}; font-size: 1.05rem;"><i class="fa-solid fa-triangle-exclamation"></i> ${data.priority}</strong>
                </div>
                <div class="result-box">
                    <span class="telemetry-item">Categoría Detectada:</span><br>
                    <strong style="color: var(--azure-cyan); font-size: 1rem;"><i class="fa-solid fa-folder-tree"></i> ${data.category}</strong>
                </div>
            </div>

            <div class="grid-2col mt-2">
                <div class="result-box">
                    <span class="telemetry-item">Estado Emocional del Usuario:</span><br>
                    <strong style="color: #e5e7eb;">${data.sentiment}</strong>
                </div>
                <div class="result-box">
                    <span class="telemetry-item">Tiempo Estimado de Resolución:</span><br>
                    <strong style="color: #10b981;">${data.estimated_resolution}</strong>
                </div>
            </div>

            <div class="result-box mt-3">
                <h4 style="color: var(--azure-cyan);"><i class="fa-solid fa-lightbulb"></i> Solución Técnica Sugerida por la IA:</h4>
                <p class="mt-2" style="font-size: 0.95rem; line-height: 1.5; color: #e5e7eb;">${data.ai_solution}</p>
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
        visionDescription.innerText = 'Enviando captura de pantalla a Azure Computer Vision MLaaS...';
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
            visionDescription.innerText = 'Error al procesar la captura de pantalla.';
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
                document.getElementById('t-python').innerText = data.python_version || '.NET 8.0 (C#)';
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
            liveStatusText.innerText = 'Servidor Smart-Support Helpdesk (Listo para Azure App Service)';
        }
    }
});
