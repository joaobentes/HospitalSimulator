using Microsoft.EntityFrameworkCore;

namespace HospitalSimulator.Models 
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DoctorRole> DoctorRoles { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<TreatmentMachine> TreatmentMachines { get; set; }
        public DbSet<TreatmentRoom> TreatmentRooms { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorRole>()
                .HasKey(dr => new { dr.DoctorID , dr.RoleID });

            modelBuilder.Entity<DoctorRole>()
                .HasOne(d => d.Doctor)
                .WithMany(dr => dr.DoctorRoles)
                .HasForeignKey(d => d.DoctorID);

            modelBuilder.Entity<DoctorRole>()
                .HasOne(r => r.Role)
                .WithMany(dr => dr.DoctorRoles)
                .HasForeignKey(r => r.RoleID);
        }
    }
}