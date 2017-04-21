using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalSimulator.Models {
    public class Consultation {
        public int ConsultationID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ConsultationDate { get; set; }
        
        [Required(ErrorMessage = "Patient's ID is required.")]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Required(ErrorMessage = "Doctor's ID is required.")]
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        [Required(ErrorMessage = "Treatment Machine's Name is required.")]
        [ForeignKey("TreatmentRoom")]
        public int TreatmentRoomName { get; set; }
        public virtual TreatmentRoom TreatmentRoom { get; set; }
    }
}