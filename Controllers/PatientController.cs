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
                return CreatedAtRoute("ScheduleConsultation", new { id = patient.PatientID}, patient);
            }
        }
    }
}