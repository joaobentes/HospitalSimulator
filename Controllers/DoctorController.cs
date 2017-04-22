using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

using HospitalSimulator.Models;

namespace HospitalSimulator.Controllers {
    [Route("doctors")]
    public class DoctorController: Controller {
        private readonly ApplicationDbContext _context;
        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Doctor> GetAll()
        {
            return _context.Doctors.ToList();
        }

        [HttpGet("{id}", Name = "GetDoctor")]
        public IActionResult GetByID(long id)
        {
            Console.WriteLine("Getting Doctor");
            var doctor = _context.Doctors.Where(d => d.DoctorID == id);
            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(doctor);
            }
        }

        [HttpPost]
        public IActionResult Add ([FromBody] Doctor doctor)
        {
            if(doctor == null)
            {
                Console.WriteLine("Could not add the doctor");
                return BadRequest();          
            }
            else
            {
                if(doctor.DoctorRoles == null)
                {
                    doctor.DoctorRoles = new List<DoctorRole>();
                }
                foreach(var roleName in doctor.Roles)
                {
                    var role = _context.Roles.ToList().Find(r => r.Name == roleName);
                    var doctorRole = new DoctorRole { RoleID = role.RoleID };
                    doctor.DoctorRoles.Add(doctorRole);
                }

                Console.WriteLine("Trying to add the doctor");
                _context.Doctors.Add(doctor);
                
                Console.WriteLine("Trying to save everything");
                _context.SaveChanges();
                
                Console.WriteLine("Saved");

                var returnObject = new Doctor {
                    DoctorID = doctor.DoctorID,
                    Name = doctor.Name,
                    Roles = doctor.Roles
                };

                return new ObjectResult(returnObject);
            }
        }
    }
}