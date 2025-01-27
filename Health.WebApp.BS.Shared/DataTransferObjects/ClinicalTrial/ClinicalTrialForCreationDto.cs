using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace HealthManager.WebApp.BS.Shared.DataTransferObjects.Health
{
    public class ClinicalTrialForCreationDto
    {
        [Required(ErrorMessage = "Title is a required field.")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "TrialId is a required field.")]
        [JsonPropertyName("trialId")]
        public string TrialId { get; set; }

        [Required(ErrorMessage = "Start Date is a required field.")]
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("endDate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("participants")]
        [Range(1, int.MaxValue, ErrorMessage = "Participants must be at least 1.")]
        public int Participants { get; set; } = 1;

    }
}
