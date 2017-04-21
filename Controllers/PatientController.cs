using Microsoft.AspNetCore.Mvc;
using HospitalSimulator.Models;
using System.Collections.Generic;

namespace HospitalSimulator.Controllers {
    [Route("patients")]
    public class PatientController: Controller {
        
        [HttpGet]
        public IEnumerable<Patient> GetAll()
        {
            return Repository.Instance.Patients;
        }
        
        [HttpGet("{id}", Name = "GetPatient")]
        public IActionResult GetByID(long id)
        {
            var patient = Repository.Instance.Patients.Find(p => p.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(patient);
            }
        }

        [HttpPost]
        public IActionResult Add ([FromBody] Patient patient)
        {
            if(patient == null)
            {
                return BadRequest();
            }
            else
            {
                Repository.Instance.Patients.Add(patient);
                return CreatedAtRoute("GetPatient", new { id = patient.PatientID}, patient);
            }
        }
    }
}