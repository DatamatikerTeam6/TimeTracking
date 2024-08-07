using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DogRallyMVC.Models
{
    public class TimetrackingDTO
    {
        // Serialization
        // Validation: Data Annotation
        // Declaration of public properties
               
        [JsonPropertyName("date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;

        [JsonPropertyName("hoursworked")]
        [Required]
        [Range(0, 24)]
        public double HoursWorked { get; set; }


        [JsonPropertyName("userID")]
        [Required]
        public string UserID { get; set; }

        // Tilføjet denne egenskab
        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; } = "default@example.com";
    }
}
