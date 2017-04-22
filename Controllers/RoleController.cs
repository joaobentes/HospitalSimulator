using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using HospitalSimulator.Models;

namespace HospitalSimulator.Controllers {
    [Route("roles")]
    public class RoleController: Controller {
        private readonly ApplicationDbContext _context;
        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.ToList();
        }

        [HttpGet("{name}", Name = "GetDoctorRole")]
        public IActionResult GetByID(string name)
        {
            var role = _context.Roles.Where(d => d.Name == name);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(role);
            }
        }

        [HttpPost]
        public IActionResult Add ([FromBody] Role role)
        {
            if(role == null)
            {
                return BadRequest();                
            }
            else
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
                return CreatedAtRoute("GetDoctorRole", new { name = role.Name}, role);
            }
        }
    }
}