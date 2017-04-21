using System.ComponentModel.DataAnnotations;

namespace HospitalSimulator.Models {
    public class TreatmentMachine {

        [Key]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Treatment Machine's Capability is required.")]
        public string Capability { get; set; }        
    }
}