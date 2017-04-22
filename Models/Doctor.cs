using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HospitalSimulator.Models {
    public class Doctor 
    {   
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Doctor's Name is required.")]
        public string Name { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Doctor's Roles are required.")]
        public string[] Roles { get; set; }
        
        public virtual List<DoctorRole> DoctorRoles { get; set; }
    }
}