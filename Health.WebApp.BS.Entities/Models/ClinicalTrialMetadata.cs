using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HealthManager.WebApp.BS.Entities.Models
{
    public partial class ClinicalTrialMetadata
    {           

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("trialId")]
        [Required]
        public string TrialId { get; set; }

        [JsonPropertyName("title")]
        [Required]
        public string Title { get; set; }

        [JsonPropertyName("startDate")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("status")]
        [Required]
        [EnumDataType(typeof(Statuses),ErrorMessage = "Invalid status value. Allowed values: NotStarted, Ongoing, Completed.")]
        public string Status { get; set; }

        [JsonPropertyName("endDate")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("participants")]
        [Range(1, int.MaxValue, ErrorMessage = "Participants must be at least 1.")]
        public int? Participants { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }


        public static readonly string[] ValidStatuses = { "Not Started", "Ongoing", "Completed" };


        public void TransformData()
        {
            // Rule 1: Duration Calculation
            if (StartDate != default && EndDate.HasValue)
            {
                Duration = (EndDate.Value - StartDate).Days;
            }

            // Rule 2: Default endDate for "Ongoing" trials
            if (string.Equals(Status, "Ongoing", StringComparison.OrdinalIgnoreCase) && !EndDate.HasValue)
            {
                EndDate = StartDate.AddMonths(1);
                Duration = (EndDate.Value - StartDate).Days;
            }        
        }

        public enum Statuses
        {
            [Display(Name = "Not Started")]
            NotStarted,

            [Display(Name = "Ongoing")]
            Ongoing,

            [Display(Name = "Completed")]
            Completed
        }

    }
}
