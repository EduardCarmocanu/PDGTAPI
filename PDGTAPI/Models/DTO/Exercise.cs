using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.Models.DTO
{
    public class Exercise
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guide Guide { get; set; }
        public int Weight { get; set; }
        [Required]
        public int Repetitions { get; set; }
        [Required]
        public int Sets { get; set; }
    }
}
