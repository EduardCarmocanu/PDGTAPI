
using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.DTO
{
    public class GuideDTO
    {
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
