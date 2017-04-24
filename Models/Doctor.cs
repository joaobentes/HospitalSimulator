using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HospitalSimulator.Models 
{
    public class Doctor 
    {   
        public string DoctorID { get; set; }

        public string Name { get; set; }

        public List<DoctorRole> Roles { get; set; }
        
    }
}