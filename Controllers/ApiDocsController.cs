using Microsoft.AspNetCore.Mvc;

namespace ProyectoAzure.Controllers
{
    [ApiController]
    [Route("api/docs")]
    public class ApiDocsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetApiDocs()
        {
            return Ok(new
            {
                service_name = "Azure AI Hub REST API (ASP.NET Core C#)",
                architecture_type = "PaaS RESTful Service (.NET 8)",
                version = "1.0.0",
                endpoints = new[]
                {
                    new
                    {
                        endpoint = "/api/azure-info",
                        method = "GET",
                        description = "Devuelve telemetría en vivo del entorno Azure App Service y la Matriz de Responsabilidad Compartida en .NET."
                    },
                    new
                    {
                        endpoint = "/api/analyze-text",
                        method = "POST",
                        payload_example = new { text = "Texto a procesar" },
                        description = "API de Inteligencia Artificial para análisis de sentimiento, palabras clave y resumen ejecutivo."
                    },
                    new
                    {
                        endpoint = "/api/analyze-image",
                        method = "POST",
                        payload_example = "multipart/form-data (archivo) o JSON {'url': 'https://...'}",
                        description = "API de Visión por Computadora MLaaS en C#."
                    }
                }
            });
        }
    }
}
