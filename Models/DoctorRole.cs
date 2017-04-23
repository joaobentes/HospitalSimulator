namespace HospitalSimulator.Models 
{
    public class DoctorRole
    {
        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public string RoleName { get; set; }
        public Role Role { get; set; }
    }
}