using System.ComponentModel.DataAnnotations;

namespace HospitalSimulator.Models 
{
    public class Patient 
    {
        public string PatientID { get; set; }

        [Required(ErrorMessage = "Patient's Name is required.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Patient's Condition is required.")]
        public string Condition { get; set; }
    }
}