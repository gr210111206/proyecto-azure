using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ProyectoAzure.Controllers
{
    [ApiController]
    [Route("api/azure-info")]
    public class AzureInfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAzureInfo()
        {
            var siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "Smart-Support-Helpdesk-Dev";
            var region = Environment.GetEnvironmentVariable("REGION_NAME") ?? "East US (Azure Default)";
            var sku = Environment.GetEnvironmentVariable("WEBSITE_SKU") ?? "Free (F1) / Shared";
            var dotnetVer = Environment.Version.ToString();

            var paasMatrix = new
            {
                managed_by_azure = new[]
                {
                    "Infraestructura Física de Datacenters y Energía Redundante",
                    "Servidores Físicos y Capa de Virtualización Hyper-V",
                    "Sistema Operativo Host (Linux/Windows) y Parches de Seguridad",
                    "Entorno de Ejecución ASP.NET Core 8.0 (.NET Runtime)",
                    "Balanceador de Carga y Auto-escalamiento en Picos de Tickets",
                    "Certificación SSL/TLS HTTPS y Protección DDoS de Red"
                },
                managed_by_user = new[]
                {
                    "Código Fuente del Sistema Helpdesk (C# / ASP.NET Core)",
                    "Lógica de Triaje de Tickets e Integración con Modelos de IA",
                    "Interfaz Web de Usuario (HTML/CSS/JS con Glassmorphism)",
                    "Configuración de Variables de Entorno de Soporte Técnico",
                    "Repositorio de Código en GitHub y Canalización CI/CD"
                }
            };

            return Ok(new
            {
                status = "online",
                timestamp = DateTime.UtcNow.ToString("o"),
                app_name = siteName,
                region = region,
                sku = sku,
                platform_type = "PaaS - Azure App Service (.NET 8)",
                os = $"{RuntimeInformation.OSDescription}",
                python_version = $".NET {dotnetVer} (C# ASP.NET Core)",
                paas_responsibility_matrix = paasMatrix
            });
        }
    }
}
