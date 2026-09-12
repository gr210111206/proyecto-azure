namespace ProyectoAzure.Models
{
    public class SupportTicketRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Department { get; set; } = "TI / Soporte Técnico";
    }

    public class ImageErrorAnalysisRequest
    {
        public string Url { get; set; } = string.Empty;
    }
}
