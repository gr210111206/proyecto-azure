namespace ProyectoAzure.Models
{
    public class TextAnalysisRequest
    {
        public string Text { get; set; } = string.Empty;
    }

    public class ImageAnalysisUrlRequest
    {
        public string Url { get; set; } = string.Empty;
    }
}
