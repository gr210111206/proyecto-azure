using Microsoft.AspNetCore.Mvc;
using ProyectoAzure.Models;
using System.Text.RegularExpressions;

namespace ProyectoAzure.Controllers
{
    [ApiController]
    [Route("api/analyze-text")]
    public class TextAnalysisController : ControllerBase
    {
        [HttpPost]
        public IActionResult AnalyzeText([FromBody] TextAnalysisRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Text))
            {
                return BadRequest(new { error = "Por favor ingresa un texto para analizar." });
            }

            var text = request.Text.Trim();
            var words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int wordCount = words.Length;
            int charCount = text.Length;

            var positiveWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "excelente", "bueno", "gran", "eficiente", "rápido", "seguro", "innovador", "éxito", "optimizado", "paas", "azure", "ventaja", "fácil"
            };

            var negativeWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "lento", "error", "falla", "costoso", "problema", "difícil", "caída", "riesgo", "inseguro", "vulnerabilidad"
            };

            var textLower = text.ToLower();
            int posScore = words.Count(w => positiveWords.Contains(w.ToLower()));
            int negScore = words.Count(w => negativeWords.Contains(w.ToLower()));

            string sentiment;
            double sentimentScore;

            if (posScore > negScore)
            {
                sentiment = "Positivo";
                sentimentScore = Math.Min(0.6 + (posScore * 0.1), 0.98);
            }
            else if (negScore > posScore)
            {
                sentiment = "Negativo";
                sentimentScore = Math.Min(0.6 + (negScore * 0.1), 0.95);
            }
            else
            {
                sentiment = "Neutral / Técnico";
                sentimentScore = 0.85;
            }

            // Keyword Extraction
            var matches = Regex.Matches(textLower, @"\b[a-zA-ZáéíóúÁÉÍÓÚñÑ]{4,}\b");
            var stopwords = new HashSet<string> { "para", "como", "esta", "este", "entre", "sobre", "todo", "donde", "desde", "hasta", "hacia", "cada", "pero", "mas" };

            var filteredWords = matches
                .Select(m => m.Value)
                .Where(w => !stopwords.Contains(w))
                .GroupBy(w => w)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => char.ToUpper(g.Key[0]) + g.Key.Substring(1))
                .ToList();

            // Summary
            var sentences = Regex.Split(text, @"[.!?]+")
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            string summary = sentences.Count > 2 ? $"{sentences[0]}. {sentences[1]}." : text;

            var aiInsights = new List<string>
            {
                $"El texto aborda conceptos clave como {(filteredWords.Any() ? string.Join(", ", filteredWords.Take(3)) : "tecnología")}.",
                $"Se detectó un tono de comunicación **{sentiment}** con una confianza del {(int)(sentimentScore * 100)}%.",
                "Recomendación PaaS: Los modelos de lenguaje como este se pueden consumir eficientemente desde Azure App Service en C# (.NET 8)."
            };

            return Ok(new
            {
                status = "success",
                word_count = wordCount,
                char_count = charCount,
                summary = summary,
                sentiment = sentiment,
                sentiment_score = Math.Round(sentimentScore * 100, 1),
                keywords = filteredWords,
                ai_insights = aiInsights,
                processed_by = "Azure Cloud AI Engine (C# / .NET 8 PaaS)"
            });
        }
    }
}
