using System;
using System.ComponentModel.DataAnnotations;

namespace PDGTAPI.DTO
{
    public class UserDataDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime BaselineDate { get; set; }
        [Required]
        public char RandomisationGroup { get; set; }
    }
}
