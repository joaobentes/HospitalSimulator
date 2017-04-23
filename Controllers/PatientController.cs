using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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

        [HttpPost]
        public IActionResult Create ([FromBody] Patient patient)
        {
            if(patient == null)
            {
                return BadRequest("Invalid Patient object");
            }
            else
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                var consultationController = new ConsultationController(_context);
                return consultationController.Create(new Consultation { PatientID = patient.PatientID });
            }
        }
    }
}