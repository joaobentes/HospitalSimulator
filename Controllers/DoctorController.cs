using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

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
        public IEnumerable<JObject> GetAll()
        {
            var doctors =  _context.Doctors.Include(d => d.Roles).ToList();
            var resultSet = new List<JObject>();

            foreach (var doctor in doctors)
            {
                var doctorJson = new JObject();
                
                doctorJson.Add("name", doctor.Name);
                
                // Get only the names of the doctor roles and covert it to JArray
                var doctorRoles = new JArray(doctor.Roles.Select(r => r.RoleName).ToArray());
                doctorJson.Add("roles", doctorRoles);

                resultSet.Add(doctorJson);
            }
            return resultSet;
        }

        [HttpPost]
        public IActionResult Add ([FromBody] JObject payload)
        {
            if(payload == null)
            {
                return BadRequest();          
            }

            var name = payload.GetValue("name").ToString();
            var roles = payload.GetValue("roles").ToList().Select(r => r.ToString()).ToList();

            // Check if the all the roles exist
            foreach (var role in roles)
            {
                if(!_context.Roles.ToList().Exists(r => r.Name == role))
                {
                    return BadRequest();
                }
            }

            var doctor = new Doctor {
                Name = name,
                Roles = new List<DoctorRole>()
            };

            foreach (var roleName in roles)
            {
                doctor.Roles.Add(new DoctorRole 
                {
                    RoleName = roleName,
                    DoctorID = doctor.DoctorID
                });
            }

            // Save the doctor object
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            // Get only the names of the doctor roles and covert it to JArray
            var doctorRoles = new JArray(doctor.Roles.Select(r => r.RoleName).ToArray());

            // Create result object
            var resultObject = new JObject();
            resultObject.Add("name", doctor.Name);
            resultObject.Add("roles", doctorRoles);

            return new ObjectResult(resultObject);
        }
    }
}