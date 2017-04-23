using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using HospitalSimulator.Models;

namespace HospitalSimulator.Controllers 
{

    [Route("consultations")]
    public class ConsultationController: Controller 
    {
        private readonly ApplicationDbContext _context;
        public ConsultationController(ApplicationDbContext context)
        {
            _context = context;
        }    

        [HttpGet]
        public IEnumerable<JObject> GetAll()
        {
            List<Consultation> consultations = _context.Consultations
                                                .Include(c => c.Doctor)
                                                .Include(c => c.Patient)
                                                .Include(c => c.TreatmentRoom)
                                                .ToList();
            // Prepare the result set object
            List<JObject> resultSet = new List<JObject>();
            foreach (var c in consultations)
            {
                resultSet.Add(BuildResultObject(c));
            }
            return resultSet;
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
            
            // Control variables - For all use cases
            var hasFound = false;
            var availableRooms = new List<TreatmentRoom>();
            var availableDoctors = new List<Doctor>();
            var consultationDate = new DateTime();
                        
            // Find the next available date, available doctors and rooms.
            if(patient.Condition.Contains("Cancer"))
            {
                var oncologists = doctors.FindAll(d => d.Roles.Exists( r => r.RoleName  == "Oncologist"));
                
                if(patient.Condition == "Cancer.Head&Neck")
                {
                    var advancedRooms = rooms.FindAll(r => r.Machine != null && r.Machine.Capability == "Advanced");

                    while(!hasFound)
                    {
                        var consultationsPerDoctor = consultations.FindAll(c => doctors.Exists(d => d.DoctorID == d.DoctorID)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        var consultationsPerRoom = consultations.FindAll(c => advancedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        // Prepare available rooms and doctors candidates
                        availableRooms.Clear();
                        availableDoctors.Clear();
                        availableRooms.AddRange(advancedRooms);
                        availableDoctors.AddRange(oncologists);

                        // Check available doctors
                        foreach (var c in consultationsPerDoctor)
                        {
                            availableDoctors.Remove(c.Doctor);
                        }
                        // Check available rooms
                        foreach (var c in consultationsPerRoom)
                        {
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
                else if(patient.Condition == "Cancer.Breast")
                {
                    var equippedRooms = rooms.FindAll(r => (r.MachineName != null || r.MachineName != ""));

                    while(!hasFound)
                    {
                        var consultationsPerDoctor = consultations.FindAll(c => doctors.Exists(d => d.DoctorID == d.DoctorID)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        var consultationsPerRoom =  consultations.FindAll(c => equippedRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                        // Prepare available rooms and doctors candidates
                        availableRooms.Clear();
                        availableDoctors.Clear();
                        availableRooms.AddRange(equippedRooms);
                        availableDoctors.AddRange(oncologists);

                        // Check available doctors
                        foreach (var c in consultationsPerDoctor)
                        {
                            availableDoctors.Remove(c.Doctor);
                        }
                        // Check available rooms
                        foreach (var c in consultationsPerRoom)
                        {
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
                    var consultationsPerDoctor = consultations.FindAll(c => doctors.Exists(d => d.DoctorID == d.DoctorID)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                    var consultationsPerRoom = consultations.FindAll(c => basicRooms.Exists(r => r.Name == c.TreatmentRoomName)
                                                                        && c.ConsultationDate.Equals(potentialDate));
                    // Prepare available rooms and doctors candidates
                    availableRooms.Clear();
                    availableDoctors.Clear();
                    availableDoctors.AddRange(generals);
                    availableRooms.AddRange(basicRooms);                                                                                            
                    
                    // Check available doctors
                    foreach (var c in consultationsPerDoctor)
                    {
                        availableDoctors.Remove(c.Doctor);
                    }
                    // Check available rooms
                    foreach (var c in consultationsPerRoom)
                    {
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

            return new ObjectResult(BuildResultObject(newConsultation));
        }

        private JObject BuildResultObject(Consultation consultation)
        {
            // Prepare the doctor json object
            var doctor = _context.Doctors.Include(d => d.Roles).ToList().Where(d => d.DoctorID == consultation.Doctor.DoctorID).First();
            var doctorJson = new JObject();
            doctorJson.Add("name", doctor.Name);
            doctorJson.Add("roles", new JArray(doctor.Roles.Select(r => r.RoleName).ToArray()));

            // Prepare the treatment room object
            var room = _context.TreatmentRooms.Include(t => t.Machine).ToList().Where(t => t.Name == consultation.TreatmentRoomName).First();

            // Create the result set object
            var resultObject = new JObject();
            resultObject.Add("consultationID", consultation.ConsultationID);
            resultObject.Add("registrationDate", consultation.RegistrationDate);
            resultObject.Add("consultationDate", consultation.ConsultationDate);
            resultObject.Add("patientID", consultation.PatientID);
            resultObject.Add("patient", JObject.FromObject(consultation.Patient));
            resultObject.Add("doctorID", consultation.DoctorID);
            resultObject.Add("doctor", doctorJson);
            resultObject.Add("treatmentRoomName", consultation.TreatmentRoomName);
            resultObject.Add("treatmentRoom", JObject.FromObject(room));

            return resultObject;
        }
    }    
}