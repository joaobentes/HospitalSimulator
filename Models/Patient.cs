using System.ComponentModel.DataAnnotations;

namespace HospitalSimulator.Models 
{
    public class Patient 
    {
        public string PatientID { get; set; }

        public string Name { get; set; }
        
        public string Condition { get; set; }
    }
}