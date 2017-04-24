using System.ComponentModel.DataAnnotations;

namespace HospitalSimulator.Models 
{
    public class Patient 
    {
        public string PatientID { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Condition { get; set; }
    }
}