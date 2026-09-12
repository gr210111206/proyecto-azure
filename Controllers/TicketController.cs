using Microsoft.AspNetCore.Mvc;
using ProyectoAzure.Models;
using System.Text.RegularExpressions;

namespace ProyectoAzure.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreateTicket([FromBody] SupportTicketRequest request)
        {
            if (request == null || (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Description)))
            {
                return BadRequest(new { error = "Por favor ingresa la descripción del problema técnico para el ticket." });
            }

            var fullText = $"{request.Title} {request.Description}".Trim();
            var textLower = fullText.ToLower();

            // AI Ticket Priority & Sentiment Detection
            string priority;
            string priorityColor;
            string sentiment;
            int estResolutionMinutes;

            var criticalKeywords = new[] { "caída", "caida", "caido", "servidor", "bloqueado", "urgen", "grave", "500", "error fatal", "no responde", "perdid", "hackeo", "base de datos" };
            var highKeywords = new[] { "lento", "fallo", "falla", "login", "acceso", "contraseña", "red", "internet", "impresora", "permiso" };

            int criticalScore = criticalKeywords.Count(k => textLower.Contains(k));
            int highScore = highKeywords.Count(k => textLower.Contains(k));

            if (criticalScore > 0)
            {
                priority = "CRÍTICA (P1)";
                priorityColor = "danger";
                sentiment = "Frustrado / Urgente";
                estResolutionMinutes = 15;
            }
            else if (highScore > 0)
            {
                priority = "ALTA (P2)";
                priorityColor = "warning";
                sentiment = "Preocupado";
                estResolutionMinutes = 60;
            }
            else
            {
                priority = "NORMAL (P3)";
                priorityColor = "info";
                sentiment = "Neutral";
                estResolutionMinutes = 240;
            }

            // Category Identification
            string category;
            if (textLower.Contains("base de datos") || textLower.Contains("sql") || textLower.Contains("db"))
                category = "Base de Datos & Almacenamiento";
            else if (textLower.Contains("red") || textLower.Contains("internet") || textLower.Contains("wifi") || textLower.Contains("conexion"))
                category = "Redes & Comunicaciones";
            else if (textLower.Contains("login") || textLower.Contains("acceso") || textLower.Contains("contraseña") || textLower.Contains("password"))
                category = "Autenticación & Cuentas";
            else
                category = "Sistemas & Aplicaciones Cloud";

            // AI Recommended Solution
            string solution;
            if (category.Contains("Base de Datos"))
                solution = "Revisar el pool de conexiones en Azure SQL, verificar estados de transacciones bloqueadas y reiniciar el servicio de base de datos.";
            else if (category.Contains("Redes"))
                solution = "Verificar reglas de Firewall de Azure y NSG (Network Security Groups), comprobar latencia del gateway de red.";
            else if (category.Contains("Autenticación"))
                solution = "Forzar restablecimiento de token JWT / OAuth2 en Azure Active Directory (Microsoft Entra ID) y desbloquear cuenta.";
            else
                solution = "Reiniciar el App Service en Azure Portal, purgar caché distribuida y verificar logs de aplicación en App Insights.";

            var randomId = new Random().Next(1000, 9999);

            return Ok(new
            {
                status = "success",
                ticket_id = $"TCK-AZURE-{randomId}",
                timestamp = DateTime.UtcNow.ToString("g"),
                title = string.IsNullOrEmpty(request.Title) ? "Reporte de Falla en Sistema Cloud" : request.Title,
                description = request.Description,
                category = category,
                priority = priority,
                priority_color = priorityColor,
                sentiment = sentiment,
                estimated_resolution = $"{estResolutionMinutes} minutos",
                ai_solution = solution,
                processed_by = "Smart-Support AI Engine (Azure App Service PaaS .NET 8)"
            });
        }
    }
}
