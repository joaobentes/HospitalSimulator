namespace HospitalSimulator.Models 
{
    public class DoctorRole
    {
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }
    }
}