using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.DTO
{
    public class ExerciseDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public byte Repetitions { get; set; }
        [Required]
        public byte Sets { get; set; }
    }
}
