using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDGTAPI.Models
{
	public class PatientRegistration
	{
		[Required]
		[StringLength(64, ErrorMessage = "First Name should be at least 2 characters and maximum 64 characters", MinimumLength = 2)]
		public string FirstName { get; set; }
		[Required]
		[StringLength(64, ErrorMessage = "Last Name should be at least 2 characters and maximum 64 characters", MinimumLength = 2)]
		public string LastName { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
		public int RedCapRecordId { get; set; }
	}
}
