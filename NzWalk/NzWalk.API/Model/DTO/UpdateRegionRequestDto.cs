﻿using System.ComponentModel.DataAnnotations;

namespace NzWalk.API.Model.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be maximun 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be minimum 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }

    }
}
