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
                service_name = "Smart-Support AI Desk REST API",
                architecture_type = "PaaS RESTful Service (.NET 8)",
                version = "1.0.0",
                endpoints = new[]
                {
                    new
                    {
                        endpoint = "/api/tickets/create",
                        method = "POST",
                        payload_example = new { title = "Error 500 en Login", description = "No puedo ingresar al sistema de base de datos" },
                        description = "Crea un ticket de soporte técnico y procesa prioridad, sentimiento y solución con IA."
                    },
                    new
                    {
                        endpoint = "/api/azure-info",
                        method = "GET",
                        description = "Devuelve la telemetría del servidor en Azure App Service y la Matriz de Responsabilidad Compartida PaaS."
                    },
                    new
                    {
                        endpoint = "/api/analyze-image",
                        method = "POST",
                        payload_example = "multipart/form-data (captura de pantalla de error)",
                        description = "API MLaaS de Visión por Computadora para diagnóstico visual de fallas."
                    }
                }
            });
        }
    }
}
