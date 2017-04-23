using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
            return _context.Consultations
                    .Include(c => c.Doctor)
                    .Include(c => c.Patient)
                    .Include(c => c.TreatmentRoom)
                    .ToList();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Consultation consultation)
        {
            if(consultation == null)
            {
                return BadRequest("Invalid Consultation object");
            }

            // Get patient from id
            var patient = (Patient) _context.Patients.Where(p => p.PatientID == consultation.PatientID).First();

            if(patient == null)
            {
                return BadRequest("Patient not found");
            }
   
            // To reduce the number of db calls, the current state of the database is stored in lists
            var doctors = _context.Doctors.Include(d => d.Roles).ToList();
            var rooms = _context.TreatmentRooms.Include(t => t.Machine).ToList();
            var consultations = _context.Consultations.Where(c => c.ConsultationDate > DateTime.Now).ToList();

            // Always start trying to find from tomorrow.
            // All consultations start at 8 am.
            var potentialDate = DateTime.Now.Date.AddHours(8).AddDays(1.0);
            
            // Control variables
            var hasFound = false;
            var availableRooms = new List<TreatmentRoom>();
            var availableDoctors = new List<Doctor>();
            var consultationDate = new DateTime();
            
            // Find the next available date and register a consultation
            if(patient.Condition.Contains("Cancer"))
            {
                var oncologists = doctors.FindAll(d => d.Roles.Exists( r => r.RoleName  == "Oncologist"));
                
                if(patient.Condition == "Cancer.Head&Neck")
                {
                    var advancedRooms = rooms.FindAll(r => r.Machine != null && r.Machine.Capability == "Advanced");

                    while(!hasFound)
                    {
                        var currentConsultations = consultations.FindAll(c => oncologists.Exists(o => o.DoctorID == c.DoctorID) 
                                                                        && advancedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        // Prepare objects
                        availableRooms.Clear();
                        availableDoctors.Clear();
                        availableRooms.AddRange(advancedRooms);
                        availableDoctors.AddRange(oncologists);

                        if(currentConsultations.Count == 0)
                        {
                            consultationDate = potentialDate;
                            hasFound = true;
                        }
                        else
                        {
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
                                potentialDate = potentialDate.AddDays(1.0);
                            }
                        }
                    }
                }
                else if(patient.Condition == "Cancer.Breast")
                {;
                    // Set the data required to find the next available time
                    var equippedRooms = rooms.FindAll(r => (r.MachineName != null || r.MachineName != ""));

                    while(!hasFound)
                    {
                        var currentConsultations = consultations.FindAll(c => oncologists.Exists(o => o.DoctorID == c.DoctorID) 
                                                                        && equippedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        // Prepare objects
                        availableRooms.Clear();
                        availableDoctors.Clear();
                        availableRooms.AddRange(equippedRooms);
                        availableDoctors.AddRange(oncologists);

                        if(currentConsultations.Count == 0)
                        {
                            consultationDate = potentialDate;
                            hasFound = true;
                        }
                        else
                        {                      
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
                                potentialDate = potentialDate.AddDays(1.0);
                            }
                        }
                    }
                }
                else
                {
                    return BadRequest("Unknown Patient Condition");
                }
            }
            else if (patient.Condition == "Flue")
            {
                var generals = doctors.FindAll(d => d.Roles.ToList().Exists( r => r.RoleName == "GeneralPractitioner"));
                var basicRooms = rooms.FindAll(r => r.MachineName == null || r.MachineName == "");
                while(!hasFound)
                {
                    var currentConsultations = consultations.FindAll(c => generals.Exists(o => o.DoctorID == c.DoctorID) 
                                                                                            && basicRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                                            && c.ConsultationDate.Equals(potentialDate));
                    // Prepare available rooms and doctors candidates
                    availableRooms.Clear();
                    availableDoctors.Clear();
                    availableDoctors.AddRange(generals);
                    availableRooms.AddRange(basicRooms);                                                                                            
                    
                    if(currentConsultations.Count == 0)
                    {
                        consultationDate = potentialDate;
                        hasFound = true;
                    }
                    else
                    {
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
                            potentialDate = potentialDate.AddDays(1.0);
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Unknown Patient Condition");
            }

            // Get the first doctor available
            var doctor = availableDoctors[0];

            // Get the first room available
            var room = availableRooms[0];

            // Create the consultation object
            var newConsultation = new Consultation();
            newConsultation.ConsultationDate = potentialDate;
            newConsultation.RegistrationDate = DateTime.Now;
            newConsultation.PatientID = patient.PatientID;
            newConsultation.DoctorID = doctor.DoctorID;
            newConsultation.TreatmentRoomName = room.Name;

            // Save changes
            _context.Consultations.Add(newConsultation);
            _context.SaveChanges();

            // Prepare the doctor object
            var doctorObject = new JObject();
            doctorObject.Add("name", doctor.Name);
            doctorObject.Add("roles", new JArray(doctor.Roles.Select(r => r.RoleName).ToArray()));

            // Create the result object
            var resultObject = new JObject();
            resultObject.Add("consultationID", newConsultation.ConsultationID);
            resultObject.Add("registrationDate", newConsultation.RegistrationDate);
            resultObject.Add("consultationDate", newConsultation.ConsultationDate);
            resultObject.Add("patientID", newConsultation.PatientID);
            resultObject.Add("patient", JObject.FromObject(newConsultation.Patient));
            resultObject.Add("doctorID", newConsultation.DoctorID);
            resultObject.Add("doctor", doctorObject);
            resultObject.Add("treatmentRoomName", newConsultation.TreatmentRoomName);
            resultObject.Add("treatmentRoom", JObject.FromObject(newConsultation.TreatmentRoom));            

            return new ObjectResult(resultObject);
        }
    }
}