using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSimulator.Models {
    public class TreatmentMachine {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Treatment Machine's Capability is required.")]
        public string Capability { get; set; }        
    }
}