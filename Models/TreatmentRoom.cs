using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSimulator.Models {
    public class TreatmentRoom {
        
        [Key]
        public string Name { get; set; }

        [ForeignKey("TreatmentMachine")]
        public string MachineName { get; set; }
        public virtual TreatmentMachine Machine { get; set; }
    
        
    }
}