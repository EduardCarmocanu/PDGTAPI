
using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Models.DTO
{
    public class Guide
    {
        [Required]
        public byte Image { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
