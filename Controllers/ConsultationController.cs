using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;

using HospitalSimulator.Models;

namespace HospitalSimulator.Controllers {

    [Route("consultations")]
    public class ConsultationController: Controller {

        private readonly ApplicationDbContext _context;
        public ConsultationController(ApplicationDbContext context)
        {
            _context = context;
        }    

        [HttpGet]
        public IEnumerable<Consultation> GetAll()
        {
            return _context.Consultations.ToList();
        }

        [HttpPost("{patientId}", Name="ScheduleConsultation")]
        private IActionResult ScheduleConsultation(string patientId)
        {
            // Get patient id
            var patient = (Patient) _context.Patients.Where(p => p.PatientID == patientId);

            if(patient == null) 
            {
                return BadRequest();
            }
   
            // To reduce the number of db calls, the current state of the database is stored in lists
            var doctors = _context.Doctors.ToList();
            var rooms = _context.TreatmentRooms.ToList();
            var consultations = _context.Consultations.Where(c => c.ConsultationDate > DateTime.Now).ToList();

            // Always start trying to find from tomorrow.
            var potentialDate = DateTime.Now.AddDays(1.0); 
            
            // Control variables
            var hasFound = false;
            var availableRooms = new List<TreatmentRoom>();
            var availableDoctors = new List<Doctor>();
            var consultationDate = new DateTime();
            
            // Find the next available date and register a consultation
            if(patient.Condition.Contains("Cancer"))
            {
                var oncologists = doctors.FindAll(d => d.Roles.Exists( r => r.Role.Name  == "Oncologist"));
                
                if(patient.Condition == "Cancer.Head&Neck")
                {
                    var advancedRooms = rooms.FindAll(r => r.Machine != null && r.Machine.Capability == "Advanced");

                    while(!hasFound)
                    {
                        var currentConsultations = consultations.FindAll(c => oncologists.Exists(o => o.DoctorID == c.DoctorID) 
                                                                        && advancedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        if(currentConsultations==null)
                        {
                            consultationDate = potentialDate;
                            hasFound = true;
                        }
                        else
                        {
                            availableRooms.Clear();
                            availableDoctors.Clear();
                            availableRooms.AddRange(advancedRooms);
                            availableDoctors.AddRange(oncologists);

                            // Check availiable rooms and doctors for the current date
                            foreach (var c in currentConsultations)
                            {
                                availableDoctors.Remove(c.Doctor);
                                availableRooms.Remove(c.TreatmentRoom);
                            }
                            
                            if(availableDoctors.Count > 0 && availableRooms.Count > 0)
                            {
                                consultationDate = potentialDate;
                                hasFound = true;
                            }
                            else
                            {
                                potentialDate.AddDays(1.0);
                            }
                        }
                    }
                }
                else if(patient.Condition == "Cancer.Breast")
                {
                    // Set the data required to find the next available time
                    var equippedRooms = rooms.FindAll(r => r.Machine != null && (r.Machine.Capability == "Advanced" || r.Machine.Capability == "Simple"));

                    while(!hasFound)
                    {
                        var currentConsultations = consultations.FindAll(c => oncologists.Exists(o => o.DoctorID == c.DoctorID) 
                                                                        && equippedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        if(currentConsultations==null)
                        {
                            consultationDate = potentialDate;
                            hasFound = true;
                        }
                        else
                        {
                            availableRooms.Clear();
                            availableDoctors.Clear();
                            availableRooms.AddRange(equippedRooms);
                            availableDoctors.AddRange(oncologists); 

                            // Check availiable rooms and doctors for the current date
                            foreach (var c in currentConsultations)
                            {
                                availableDoctors.Remove(c.Doctor);
                                availableRooms.Remove(c.TreatmentRoom);
                            }
                            
                            if(availableDoctors.Count > 0 && availableRooms.Count > 0)
                            {
                                consultationDate = potentialDate;
                                hasFound = true;
                            }
                            else
                            {
                                potentialDate.AddDays(1.0);
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else if (patient.Condition == "Flue")
            {
                var generals = doctors.FindAll(d => d.Roles.ToList().Exists( r => r.Role.Name == "GeneralPractitioner"));
                var basicRooms = rooms.FindAll(r => r.Machine == null);
                while(!hasFound)
                {
                    var currentConsultations = consultations.FindAll(c => generals.Exists(o => o.DoctorID == c.DoctorID) 
                                                                                            && basicRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                                            && c.ConsultationDate.Equals(potentialDate));
                    if(currentConsultations != null)
                    {
                        consultationDate = potentialDate;
                        hasFound = true;
                    }
                    else
                    {
                        availableRooms.Clear();
                        availableDoctors.Clear();
                        availableDoctors.AddRange(generals);
                        availableRooms.AddRange(basicRooms);

                        foreach (var c in currentConsultations)
                        {
                            availableRooms.Remove(c.TreatmentRoom);
                            availableDoctors.Remove(c.Doctor);
                        }
                        if(availableDoctors.Count > 0 && availableRooms.Count > 0)
                        {
                            consultationDate = potentialDate;
                            hasFound = true;
                        }
                        else
                        {
                            potentialDate.AddDays(1.0);
                        }
                    }
                }
            }
            else
            {
                return BadRequest();
            }

            // Get the first doctor available
            var doctor = availableDoctors[0];

            // Get the first room available
            var room = availableRooms[0];

            // Create the consultation object
            var consultation = new Consultation();
            consultation.ConsultationDate = potentialDate;
            consultation.RegistrationDate = DateTime.Now;
            consultation.PatientID = patient.PatientID;
            consultation.DoctorID = doctor.DoctorID;
            consultation.TreatmentRoomName = room.Name;

            // Save changes
            _context.Consultations.Add(consultation);
            _context.SaveChanges();

            return new ObjectResult(consultation);
        }
    }
}