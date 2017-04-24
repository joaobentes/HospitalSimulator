using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HospitalSimulator.Models 
{
    public class Role
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        
        [Required]
        public string Name { get; set; }        
        
        public virtual List<DoctorRole> Doctors { get; set; }
    }
}