using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.Product
{
    public class ClinicalTrialDto
    {
        [JsonPropertyName("trialId")]
        public string TrialId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("participants")]
        public int? Participants { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

    }
}
