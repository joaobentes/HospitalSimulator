using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSimulator.Models 
{
    public class TreatmentMachine 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Capability { get; set; }
    }
}