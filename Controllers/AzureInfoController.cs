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
            var siteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME") ?? "Azure-Local-Dev-Instance";
            var region = Environment.GetEnvironmentVariable("REGION_NAME") ?? "East US (Azure Default)";
            var sku = Environment.GetEnvironmentVariable("WEBSITE_SKU") ?? "Free (F1) / Shared";
            var dotnetVer = Environment.Version.ToString();

            var paasMatrix = new
            {
                managed_by_azure = new[]
                {
                    "Infraestructura Física y Datacenters",
                    "Servidores y Virtualización (Hyper-V)",
                    "Sistema Operativo y Parches de Seguridad",
                    "Entorno de Ejecución (.NET Core / C# Runtime)",
                    "Escalamiento Automático y Balanceo de Carga",
                    "Certificados SSL/TLS y Enrutamiento HTTPS"
                },
                managed_by_user = new[]
                {
                    "Código Fuente de la Aplicación (C# / ASP.NET Core)",
                    "Lógica de Negocio e Integración con IA",
                    "Diseño de Interfaz de Usuario (HTML/CSS/JS)",
                    "Configuración de Variables de Entorno",
                    "Repositorio de Código en GitHub (CI/CD)"
                }
            };

            return Ok(new
            {
                status = "online",
                timestamp = DateTime.UtcNow.ToString("o"),
                app_name = siteName,
                region = region,
                sku = sku,
                platform_type = "PaaS - Azure App Service (.NET)",
                os = $"{RuntimeInformation.OSDescription}",
                python_version = $".NET {dotnetVer} (C#)",
                paas_responsibility_matrix = paasMatrix
            });
        }
    }
}
