using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HospitalSimulator.Models {
    public class Doctor 
    {   
        public string DoctorID { get; set; }

        [Required(ErrorMessage = "Doctor's Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Doctor's Roles are required.")]
        public List<DoctorRole> Roles { get; set; }
        
    }
}