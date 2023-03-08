﻿using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class PointsOfInterestUpdate
    {
        [Required(ErrorMessage = "enter a value for name")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}