using System.Collections.Generic;

namespace HospitalSimulator.Models
{
    public class Repository
        {
            private static readonly Repository instance = new Repository();
            public List<Patient> Patients { get; set; }
            public List<Consultation> Consultations { get; set; }
            public List<TreatmentMachine> TreatmentMachines { get; set; }
            public List<TreatmentRoom> TreatmentRooms { get; set; }

            public static Repository Instance
            {
                get
                {
                    return instance;
                }
            }

            private Repository() {}

            public void Seed ()
            {                
                // TODO
            }
        }
}