using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.DTO
{
    public class ExerciseDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public GuideDTO Guide { get; set; }
        public byte Weight { get; set; }
        [Required]
        public byte Repetitions { get; set; }
        [Required]
        public byte Sets { get; set; }
    }
}
