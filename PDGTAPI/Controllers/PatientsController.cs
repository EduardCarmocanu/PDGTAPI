using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDGTAPI.Helpers;
using PDGTAPI.Models;
using PDGTAPI.Services;

namespace PDGTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	[Authorize(Policy = "Patients")]
    public class PatientsController : ControllerBase
    {
		private readonly IUsersService _usersService;

		public PatientsController(IUsersService usersService)
		{
			_usersService = usersService;
		}

		[HttpPost]
		[Route("/register")]
		[Authorize(Policy = "Physiotherapists")]
		public async Task<IActionResult> RegisterPatientAsync([FromBody] PatientRegistration model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			ServiceResult<string> registrationResult = await _usersService.RegisterPatientAsync(model);
			if (!registrationResult.Succeded)
				return BadRequest(registrationResult.ErrorMessage);

			return Ok();
		}

		// GET: api/Patients
		[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Patients/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Patients
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
