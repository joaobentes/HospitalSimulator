using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using HospitalSimulator.Models;

namespace HospitalSimulator.Controllers 
{
    [Route("patients")]
    public class PatientController: Controller 
    {
        private readonly ApplicationDbContext _context;

        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IEnumerable<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        [HttpGet("{id}", Name = "GetPatient")]
        public IActionResult GetByID(long id)
        {
            var patient = _context.Patients.Where(p => p.PatientID == id);
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
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return CreatedAtRoute("GetPatient", new { id = patient.PatientID}, patient);
            }
        }
    }
}