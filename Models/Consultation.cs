using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSimulator.Models 
{
    public class Consultation 
    {
        public string ConsultationID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ConsultationDate { get; set; }
        
        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public string DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("TreatmentRoom")]
        public string TreatmentRoomName { get; set; }
        public virtual TreatmentRoom TreatmentRoom { get; set; }
    }
}