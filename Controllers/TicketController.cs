using Microsoft.AspNetCore.Mvc;
using ProyectoAzure.Models;
using System;
using System.Linq;

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

            var titleStr = string.IsNullOrWhiteSpace(request.Title) ? "Reporte de Falla en Sistema Cloud" : request.Title.Trim();
            var descStr = string.IsNullOrWhiteSpace(request.Description) ? titleStr : request.Description.Trim();

            var fullText = $"{titleStr} {descStr}".Trim();
            var textLower = fullText.ToLower();

            // AI Ticket Priority & Sentiment Detection
            string priority;
            string priorityColor;
            string sentiment;
            int estResolutionMinutes;

            var criticalKeywords = new[] { 
                "caída", "caida", "caido", "servidor", "bloqueado", "urgen", "grave", 
                "500", "502", "504", "error fatal", "no responde", "perdid", "hackeo", 
                "base de datos", "pasarela", "pago", "pagos", "gateway", "timeout" 
            };
            var highKeywords = new[] { 
                "lento", "fallo", "falla", "login", "acceso", "contraseña", "red", 
                "internet", "impresora", "permiso", "api", "conexion", "conexión" 
            };

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

            // Category Identification & AI Solution Matching
            string category;
            string solution;

            if (textLower.Contains("504") || textLower.Contains("502") || textLower.Contains("pasarela") || 
                textLower.Contains("pago") || textLower.Contains("pagos") || textLower.Contains("gateway") || 
                textLower.Contains("timeout") || textLower.Contains("api"))
            {
                category = "APIs & Pasarela de Pagos (Gateway)";
                solution = "Verificar tiempo de espera (timeout) en Azure API Management, revisar estado de la pasarela de pagos externa e inspeccionar logs de respuesta HTTP 504 en Application Insights.";
            }
            else if (textLower.Contains("base de datos") || textLower.Contains("sql") || textLower.Contains("db") || textLower.Contains("mysql"))
            {
                category = "Base de Datos & Almacenamiento";
                solution = "Revisar el pool de conexiones en Azure SQL, verificar estados de transacciones bloqueadas y optimizar consultas en la base de datos.";
            }
            else if (textLower.Contains("red") || textLower.Contains("internet") || textLower.Contains("wifi") || textLower.Contains("conexion") || textLower.Contains("conexión"))
            {
                category = "Redes & Comunicaciones Cloud";
                solution = "Verificar reglas de Firewall de Azure y NSG (Network Security Groups), comprobar latencia de red y verificar el Gateway de entrada.";
            }
            else if (textLower.Contains("login") || textLower.Contains("acceso") || textLower.Contains("contraseña") || textLower.Contains("password") || textLower.Contains("token"))
            {
                category = "Autenticación & Cuentas (Entra ID)";
                solution = "Forzar restablecimiento de token JWT / OAuth2 en Microsoft Entra ID (Azure AD) y verificar permisos de acceso a la cuenta.";
            }
            else
            {
                category = "Sistemas & Aplicaciones Cloud";
                solution = "Reiniciar el App Service en Azure Portal, purgar caché distribuida y verificar logs de la aplicación en Application Insights.";
            }

            var randomId = new Random().Next(1000, 9999);

            return Ok(new
            {
                status = "success",
                ticket_id = $"TCK-AZURE-{randomId}",
                timestamp = DateTime.UtcNow.ToString("g"),
                title = titleStr,
                description = descStr,
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
