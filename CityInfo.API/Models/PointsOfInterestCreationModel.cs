using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestCreationModel
    {
        [Required(ErrorMessage = "You should enter a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}