using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HospitalSimulator.Models 
{
    public class Role
    {
        public int RoleID { get; set; }
        
        [Required(ErrorMessage = "Doctor Role's Name is required.")]
        public string Name { get; set; }        
        
        public virtual List<DoctorRole> DoctorRoles { get; set; }
    }
}